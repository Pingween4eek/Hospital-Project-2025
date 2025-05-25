#include <string>
#include "Patient.h"
#include <list>
#include <iostream>
#include <vector>
#include <sstream>
#include "send_message.h"
#include "receive_message.h"
#include "socket_wrapper.h"
#include <algorithm> 

using namespace std;

std::vector<std::string> split(const std::string& str) {
    std::vector<std::string> result;
    std::istringstream stream(str);
    std::string word;

    while (stream >> word) {
        result.push_back(word);
    }

    return result;
}

std::string join(const std::vector<std::string>& words) {
    std::string result;

    for (size_t i = 0; i < words.size(); ++i) {
        result += words[i];

        if (i < words.size() - 1) {
            result += ' ';
        }
    }

    return result;
}

SocketWrapper& operator>>(SocketWrapper& socket, Patient& pat) {
    std::string input;

    socket >> input;

    if (input == "BACK") {
        pat._days = 74;
        return socket;
    }

    std::vector<std::string> words = split(input);

    if (words.size() == 10) {
        pat._snils = std::stoi(words[0]);
        pat._name = words[1];
        pat._surname = words[2];
        pat._gender = words[3];
        pat._age = std::stoi(words[4]);
        pat._diagnosis = words[5];
        pat._status = words[6];
        pat._doctor = words[7];
        pat._department = words[8];
        pat._days = std::stoi(words[9]);
    }
    else {
        pat._days = 75;
    }
    

    return socket;
}

istream& operator >>(istream& in, Patient& pat) {
    in >> pat._snils;
    in >> pat._name;
    in >> pat._surname;
    in >> pat._gender;
    in >> pat._age;
    in >> pat._diagnosis;
    in >> pat._status;
    in >> pat._doctor;
    in >> pat._department;
    in >> pat._days;

    return in;
}

SocketWrapper& operator<<(SocketWrapper& socket, Patient& pat) {
    std::vector<std::string> words;

    words.push_back(std::to_string(pat._snils));
    words.push_back(pat._name);
    words.push_back(pat._surname);
    words.push_back(pat._gender);
    words.push_back(std::to_string(pat._age));
    words.push_back(pat._diagnosis);
    words.push_back(pat._status);
    words.push_back(pat._doctor);
    words.push_back(pat._department);
    words.push_back(std::to_string(pat._days));

    std::string output = join(words);
    socket << output;

    return socket;
}

ostream& operator <<(ostream& out, Patient& pat) {
	cout << " Id: ";
	out << pat._snils << endl;
	cout << " Name: ";
	out << pat._name << endl;
	cout << " Surname: ";
	out << pat._surname << endl;
	cout << " Gender: ";
	out << pat._gender << endl;
	cout << " Age: ";
	out << pat._age << endl;
	cout << " Diagnosis: ";
	out << pat._diagnosis << endl;
	cout << " Status: ";
	out << pat._status << endl;
	cout << " Doctor: ";
	out << pat._doctor << endl;
	cout << " Department: ";
	out << pat._department << endl;
	cout << "Days:";
	out << pat._days << endl;
	return out;
}