#include <iostream>
#include "sqlite3.h"
#include <string>
#include <fstream>
#include "database.h"
#include "Patient.h"
#include <winsock2.h>
#include <ws2tcpip.h>
#include <sstream>
#include <algorithm> 
#include "send_message.h"
#include "receive_message.h"
#include <typeinfo>

bool flag = true;

std::string to_lower(const std::string& str) {
    std::string result = str;
    for (char& c : result) {
        c = std::tolower(c);
    }
    return result;
}


namespace patient_db {
    // Инициализация базы данных
    sqlite3* init_db() {
        sqlite3* db;
        int rc = sqlite3_open("ivangaog_hospita.db", &db);
        if (rc) {
            std::cerr << "Can't open database: " << sqlite3_errmsg(db) << std::endl;
            return nullptr;
        }

        const char* sql = "CREATE TABLE IF NOT EXISTS patients ("
            "snils INTEGER,"
            "surname TEXT NOT NULL,"
            "name TEXT NOT NULL,"
            "gender TEXT NOT NULL,"
            "age INTEGER,"
            "diagnosis TEXT NOT NULL,"
            "status TEXT NOT NULL,"
            "doctor TEXT NOT NULL,"
            "department TEXT NOT NULL,"
            "days_in_hospital INTEGER);";

        char* errMsg = 0;
        rc = sqlite3_exec(db, sql, 0, 0, &errMsg);
        if (rc != SQLITE_OK) {
            std::cerr << "SQL error: " << errMsg << std::endl;
            sqlite3_free(errMsg);
        }

        return db;
    }

    // Создание пациентов
    std::string create_patients(SOCKET* client_socket, std::vector<Patient>* arr) {
        SocketWrapper client(*client_socket);
        sqlite3* db = init_db();
        if (!db) return "ERROR_DB_INIT";

        const char* delete_sql = "DELETE FROM patients;";
        char* errMsg = nullptr;
        if (sqlite3_exec(db, delete_sql, nullptr, nullptr, &errMsg) != SQLITE_OK) {
            std::cerr << "Failed to delete patients: " << errMsg << std::endl;
            sqlite3_free(errMsg);
            sqlite3_close(db);
            return "ERROR_DB_DELETE";
        }

        int n = -1;

        while (n < 0) {
            std::string nt = receiveMessage(*client_socket);
            if (nt == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }

            if (nt == "BACK") {
                return "BACK";
            }

            if (std::all_of(nt.begin(), nt.end(), std::isdigit) && (!nt.empty())) {
                n = stoi(nt);
            }
            else {
                n = -1;
            }
        }

        if (n <= 0) {
            closesocket(*client_socket);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
            return "ok";
        }

        arr->clear();
        Patient patient;
        for (int i = 0; i < n; i++) {
            client >> patient;
            arr->push_back({ patient });
        }
        
        if (patient.getDays() == 75) {
            return "ERROR001RECEIVE";
        }


        const char* sql = "INSERT INTO patients (snils, surname, name, gender, age, diagnosis, "
            "status, doctor, department, days_in_hospital) "
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";

        sqlite3_stmt* stmt;
        if (sqlite3_prepare_v2(db, sql, -1, &stmt, 0) != SQLITE_OK) {
            std::cerr << "Failed to prepare statement: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return "ERROR_DB_PREPARE";
        }

        for (auto& patient : *arr) {
            sqlite3_bind_int(stmt, 1, patient.getSnils());
            sqlite3_bind_text(stmt, 2, patient.getSurname().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_text(stmt, 3, patient.getName().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_text(stmt, 4, patient.getGender().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_int(stmt, 5, patient.getAge());
            sqlite3_bind_text(stmt, 6, patient.getDia().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_text(stmt, 7, patient.getStatus().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_text(stmt, 8, patient.getDoctor().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_text(stmt, 9, patient.getDepartment().c_str(), -1, SQLITE_TRANSIENT);
            sqlite3_bind_int(stmt, 10, patient.getDays());

            if (sqlite3_step(stmt) != SQLITE_DONE) {
                std::cerr << "Execution failed: " << sqlite3_errmsg(db) << std::endl;
            }

            sqlite3_reset(stmt);
        }

        sqlite3_finalize(stmt);
        sqlite3_close(db);

        //sendMessage(client_socket, "CLOSE");
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }

    // Чтение пациентов из базы данных
    void read_patients(std::vector<Patient>* arr) {
        sqlite3* db = init_db();
        if (!db) return;

        const char* sql = "SELECT snils, surname, name, gender, age, diagnosis, "
            "status, doctor, department, days_in_hospital FROM patients;";

        sqlite3_stmt* stmt;
        if (sqlite3_prepare_v2(db, sql, -1, &stmt, 0) != SQLITE_OK) {
            std::cerr << "Failed to prepare statement: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return;
        }

        arr->clear();

        while (sqlite3_step(stmt) == SQLITE_ROW) {
            Patient patient;
            patient.setSnils(sqlite3_column_int(stmt, 0));
            patient.setSurname(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1)));
            patient.setName(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2)));
            patient.setGender(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 3)));
            patient.setAge(sqlite3_column_int(stmt, 4));
            patient.setDia(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 5)));
            patient.setStatus(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 6)));
            patient.setDoctor(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 7)));
            patient.setDepartment(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 8)));
            patient.setDays(sqlite3_column_int(stmt, 9));

            arr->push_back(patient);
        }

        sqlite3_finalize(stmt);
        sqlite3_close(db);
    }

    // Добавление пациента
    std::string add_patient(SOCKET* client_socket, std::vector<Patient>* arr) {
        SocketWrapper client(*client_socket);

        // Получаем данные пациента
        Patient patient;
        client >> patient;

        if (patient.getDays() == 75) return "ERROR001RECEIVE";
        if (patient.getDays() == 74) return "BACK";

        // Добавляем пациента в вектор
        arr->push_back(patient);

        // Работа с базой данных
        sqlite3* db = init_db();
        if (!db) return "ERROR_DB_INIT";

        // Начинаем транзакцию
        if (sqlite3_exec(db, "BEGIN TRANSACTION;", nullptr, nullptr, nullptr) != SQLITE_OK) {
            std::cerr << "Begin transaction failed: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return "ERROR_DB_TRANSACTION";
        }

        // Вставляем только нового пациента
        const char* sql = "INSERT INTO patients (snils, surname, name, gender, age, diagnosis, "
            "status, doctor, department, days_in_hospital) "
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";

        sqlite3_stmt* stmt;
        if (sqlite3_prepare_v2(db, sql, -1, &stmt, nullptr) != SQLITE_OK) {
            std::cerr << "Prepare failed: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return "ERROR_DB_PREPARE";
        }

        // Привязываем параметры только для нового пациента
        sqlite3_bind_int(stmt, 1, patient.getSnils());
        sqlite3_bind_text(stmt, 2, patient.getSurname().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_text(stmt, 3, patient.getName().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_text(stmt, 4, patient.getGender().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_int(stmt, 5, patient.getAge());
        sqlite3_bind_text(stmt, 6, patient.getDia().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_text(stmt, 7, patient.getStatus().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_text(stmt, 8, patient.getDoctor().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_text(stmt, 9, patient.getDepartment().c_str(), -1, SQLITE_TRANSIENT);
        sqlite3_bind_int(stmt, 10, patient.getDays());

        // Выполняем вставку
        if (sqlite3_step(stmt) != SQLITE_DONE) {
            std::cerr << "Insert failed: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_finalize(stmt);
            sqlite3_exec(db, "ROLLBACK;", nullptr, nullptr, nullptr);
            sqlite3_close(db);
            return "ERROR_DB_INSERT";
        }

        // Финализируем и закрываем соединение
        sqlite3_finalize(stmt);

        if (sqlite3_exec(db, "COMMIT;", nullptr, nullptr, nullptr) != SQLITE_OK) {
            std::cerr << "Commit failed: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return "ERROR_DB_COMMIT";
        }

        sqlite3_close(db);

        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }

    // Удаление пациента
    std::string delete_patient(SOCKET* client_socket, std::vector<Patient>* arr) {
        sqlite3* db = init_db();
        if (!db) return "ERROR_DB_INIT";

        SocketWrapper client(*client_socket);

        if (arr->empty()) {
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                sqlite3_close(db);
                return "ERROR001RECEIVE";
            }
            closesocket(*client_socket);
            sqlite3_close(db);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
            return "ok";
        }

        std::string patient = receiveMessage(*client_socket);
        if (patient == "ERROR001RECEIVE") {
            sqlite3_close(db);
            return "ERROR001RECEIVE";
        }
        if (patient == "BACK") {
            sqlite3_close(db);
            return "BACK";
        }

        // Удаление из базы данных
        const char* sql = "DELETE FROM patients WHERE LOWER(surname) = LOWER(?);";
        sqlite3_stmt* stmt;

        if (sqlite3_prepare_v2(db, sql, -1, &stmt, 0) != SQLITE_OK) {
            std::cerr << "Failed to prepare statement: " << sqlite3_errmsg(db) << std::endl;
            sqlite3_close(db);
            return "ERROR_DB_PREPARE";
        }

        sqlite3_bind_text(stmt, 1, patient.c_str(), -1, SQLITE_TRANSIENT);

        if (sqlite3_step(stmt) != SQLITE_DONE) {
            std::cerr << "Execution failed: " << sqlite3_errmsg(db) << std::endl;
        }

        int changes = sqlite3_changes(db);
        sqlite3_finalize(stmt);

        // Удаление из вектора
        bool deleted_from_vector = false;
        patient = to_lower(patient);

        for (auto it = arr->begin(); it != arr->end(); ) {
            if (patient == to_lower(it->getSurname())) {
                it = arr->erase(it);
                deleted_from_vector = true;
            }
            else {
                ++it;
            }
        }

        if (changes > 0 || deleted_from_vector) {
            sendMessage(client_socket, "YES");
        }
        else {
            sendMessage(client_socket, "NO");
        }

        std::string pusto = receiveMessage(*client_socket);
        if (pusto == "ERROR001RECEIVE") {
            sqlite3_close(db);
            return "ERROR001RECEIVE";
        }

        sqlite3_close(db);
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }

    // Поиск пациента
    std::string search_patient(SOCKET* client_socket, std::vector<Patient> arr) {
        SocketWrapper client(*client_socket);

        if (arr.empty()) {
            //sendMessage(client_socket, "First create list of patients with command 'create' Print 'ok' to continue");
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }

            //sendMessage(client_socket, "CLOSE");
            closesocket(*client_socket);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;

            return "ok";
        }

        //sendMessage(client_socket, "What patient you looking for? (Enter surname): ");
        std::string patient = receiveMessage(*client_socket);
        if (patient == "ERROR001RECEIVE") {
            return "ERROR001RECEIVE";
        }

        if (patient == "BACK") {
            return "BACK";
        }

        patient = to_lower(patient);
        bool flag = false;
        int id = -1;
        int n = arr.size();

        for (int i = 0; i < n; i++) {
            if (patient == to_lower(arr[i].getSurname())) {
                id = i;
                flag = true;
            }
        }
        if (flag) {
            //sendMessage(client_socket, "PATIENT");
            std::cout << arr[id];
            client << arr[id];
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }
        }
        else {
            std::cout << "There are no matches" << std::endl;
            sendMessage(client_socket, "NO PATIENT");
            //sendMessage(client_socket, "CLOSE");
            closesocket(*client_socket);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;

            return "ok";
        }
        //sendMessage(client_socket, "CLOSE");
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }

    std::string print_patients(SOCKET* client_socket, std::vector<Patient> arr) {
        SocketWrapper client(*client_socket);

        int n = arr.size();
        sendMessage(client_socket, std::to_string(n));

        for (int i = 0; i < n; i++) {
            //sendMessage(client_socket, "PATIENT");
            std::cout << arr[i];
            client << arr[i];
        }

        std::string pusto = receiveMessage(*client_socket);
        if (pusto == "ERROR001RECEIVE") {
            return "ERROR001RECEIVE";
        }
        //sendMessage(client_socket, "CLOSE");
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }
}