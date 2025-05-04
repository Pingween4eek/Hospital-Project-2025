#include "send_message.h"
#include <iostream>
#include <winsock2.h>
#include <string>

void sendMessage(SOCKET *client_socket, const std::string& message) {
    //std::cout << "message = " << message << std::endl;
    int sendResult = send(*client_socket, message.c_str(), message.length(), 0);
    //std::cout << "sendresult = " << sendResult << std::endl;
    if (sendResult == SOCKET_ERROR) {
        std::cerr << "Failed to send message: " << WSAGetLastError() << std::endl;
        closesocket(*client_socket);
    }
    else {
        std::cout << "Message sent to client: " << message << std::endl;
    }
}