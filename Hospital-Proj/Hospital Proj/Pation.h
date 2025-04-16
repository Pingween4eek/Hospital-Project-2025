#pragma once

#include <string>
#include <vector>
#include <cstring>
#include "socket_wrapper.h"

class Patient {
	int _id;
	std::string _name;
	std::string _surname;
	std::string _gender;
	int _age;
	std::string _diagnosis;
	std::string _status;
	std::string _doctor;
	std::string _department;
	int _days;

public:

	 friend std::ostream& operator <<(std::ostream& out, Patient& pat); 
	 friend std::istream& operator >>(std::istream& in, Patient& pat);

	 int getId();
	 int getAge();
	 int getDays();
	 std::string getDepartment();
	 std::string getDoctor();
	 std::string getSurname();
	 std::string getName();
	 std::string getGender();
	 std::string getDia();
	 std::string getStatus();


	 void setDays(int _days);
	 void setDepartment(std::string _department);
	 void setDoctor(std::string _doctor);
	 void setId(int _id);
	 void setName(std::string name); 
	 void setSurname(std::string surname); 
	 void setGender(std::string gender); 
	 void setAge(int age); 
	 void setDia(std::string diagnosis); 
	 void setStatus(std::string status); 

	 /// Skip day
	 void advance_day(SOCKET* client_socket, int count);

	 friend void delete_patient(SOCKET* client_socket, std::vector<Patient>* arr);
	 friend SocketWrapper& operator>>(SocketWrapper& socket, Patient& pat);
	 friend SocketWrapper& operator<<(SocketWrapper& socket, Patient& pat);
};