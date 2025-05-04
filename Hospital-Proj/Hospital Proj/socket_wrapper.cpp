#include "socket_wrapper.h"
#include <iostream>

SocketWrapper::SocketWrapper(SOCKET clientSocket) : clientSocket(clientSocket) {}

SocketWrapper::~SocketWrapper() {
    closesocket(clientSocket);
}

void SocketWrapper::send(const std::string& message) {
    ::send(clientSocket, message.c_str(), message.length(), 0);
    std::cout << "Send message: " << message << std::endl;
}

SocketWrapper& SocketWrapper::operator>>(std::string& message) {
    char buffer[1024] = { 0 };
    int recvResult = recv(clientSocket, buffer, sizeof(buffer) - 1, 0);

    if (recvResult == SOCKET_ERROR) {
        std::cerr << "Error receiving data: " << WSAGetLastError() << std::endl;
        message = "75";
        closesocket(clientSocket);
    }
    else if (recvResult == 0) {
        std::cout << "Client disconnected." << std::endl;
        message.clear();
    }
    else {
        message = std::string(buffer, recvResult);
    }
    return *this;
}

SocketWrapper& SocketWrapper::operator<<(std::string& message) {
    //std::cout << "message = " << message << std::endl;
    send(message);

    return *this;
}