#include <iostream>
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


namespace patient_db{
    std::string create_patients(SOCKET *client_socket, std::vector<Patient>* arr) {
        SocketWrapper client(*client_socket);
        //sendMessage(client_socket, "Enter number of patients = ");
        
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
                //sendMessage(client_socket, "Incorrect number! Enter number of patients = ");
                n = -1;
            }
        }
        
        if (n <= 0) {
            //sendMessage(client_socket, "CLOSE");
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

        write_patients(*arr);

        //sendMessage(client_socket, "CLOSE");
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;
        return "ok";
    }
    void write_patients(std::vector<Patient> arr) {
        if (arr.empty()) return;

        std::ofstream out;
        out.open("patients.txt");
        if (!out) return;

        int n = arr.size();
        out << n << std::endl;

        for (int i = 0; i < n; i++) {
            out << arr[i] << std::endl;
        }

        out.close();
    }

    void read_patients(std::vector<Patient>* arr) {
        std::ifstream in("patients.txt");
        if (!in) return;

        int n;
        in >> n;
        if (n <= 0) return;

        *arr = {};
        Patient patient;
        for (int i = 0; i < n; i++) {
            in >> patient;
            arr->push_back({ patient });
        }

        in.close();
    }

    std::string add_patient(SOCKET* client_socket, std::vector<Patient>* arr) {
        SocketWrapper client(*client_socket);
        if (arr->empty()) {
            //sendMessage(client_socket, "First create list of patient with command 'create'");
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }

            //sendMessage(client_socket, "CLOSE");
            closesocket(*client_socket);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;

            return "ok";
        }

        Patient patient;
        client >> patient;


        if (patient.getDays() == 75) {
            return "ERROR001RECEIVE";
        }

        if (patient.getDays() == 74) {
            return "BACK";
        }

        arr->push_back(patient);
        //sendMessage(client_socket, "Patient added successfully. Print 'ok' to continue");
        //std::string pusto = receiveMessage(*client_socket);
        //if (pusto == "ERROR001RECEIVE") {
            //return "ERROR001RECEIVE";
        //}

        //sendMessage(client_socket, "CLOSE");
        closesocket(*client_socket);
        std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;

        return "ok";
    }
    

    std::string delete_patient(SOCKET* client_socket, std::vector<Patient>* arr) {
        SocketWrapper client(*client_socket);

        if (arr->empty()) {
            //sendMessage(client_socket, "List of patients doesnt exist");
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }

            //sendMessage(client_socket, "CLOSE");
            closesocket(*client_socket);
            std::cout << "Client connection closed. Waiting for reconnection..." << std::endl;

            return "ok";
        }

        //sendMessage(client_socket, "Enter the surname of patient you want to delete: ");
        std::string patient = receiveMessage(*client_socket);
        if (patient == "ERROR001RECEIVE") {
            return "ERROR001RECEIVE";
        }
        if (patient == "BACK") {
            return "BACK";
        }

        int id;
        int n = arr->size();
        patient = to_lower(patient);
        bool flag = false;

        for (int i = 0; i < n; i++) {
            if (patient == to_lower((*arr)[i].getSurname())) {
                id = i;
                flag = true;
            }
        }
        if (flag) {
            //sendMessage(client_socket, "Patient was delete, enter 'ok' to continue");
            arr->erase(arr->begin() + id);
            sendMessage(client_socket, "YES");
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }
        }

        else {
            //sendMessage(client_socket, "This patient doesn't exist, enter 'ok' to continue");
            sendMessage(client_socket, "NO");
            std::string pusto = receiveMessage(*client_socket);
            if (pusto == "ERROR001RECEIVE") {
                return "ERROR001RECEIVE";
            }

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