#include <iostream>
#include <string>
#include <ctime>
#include "Patient.h"
#include "send_message.h"
#include "receive_message.h"

int Patient::getSnils() { return _snils; }
std::string Patient::getName() { return _name; }
std::string Patient::getSurname() { return _surname; }
std::string Patient::getGender() { return _gender; }
int Patient::getAge() { return _age; }
std::string Patient::getDia() { return _diagnosis; }
std::string Patient::getStatus() { return _status; }
std::string Patient::getDoctor() { return _doctor; }
std::string Patient::getDepartment() { return _department; }
int Patient::getDays() { return _days; }

void Patient::setSnils(int snils) { _snils = snils; }
void Patient::setName(std::string name) { _name = name; }
void Patient::setSurname(std::string surname) { _surname = surname; }
void Patient::setGender(std::string gender) { _gender = gender; }
void Patient::setAge(int age) { _age = age; }
void Patient::setDia(std::string diagnosis) { _diagnosis = diagnosis; }
void Patient::setStatus(std::string status) { _status = status; }
void Patient::setDoctor(std::string doctor) { _doctor = doctor; }
void Patient::setDepartment(std::string department) { _department = department; }
void Patient::setDays(int days) { _days = days; }

void Patient::advance_day(SOCKET* client_socket, int count) {
    if (!(_status == "discharged" || _status == "died")) {
        _days -= count;
        if (_days <= 0) {
            _days = 0;
            int a = rand() % 10;
            _status = (a < 9) ? "discharged" : "died";
        }
    }
}