#pragma once

#include <winsock2.h>
#include <string>

class SocketWrapper {
public:
    explicit SocketWrapper(SOCKET clientSocket);   // Конструктор для инициализации сокета
    ~SocketWrapper();                              // Деструктор для закрытия сокета

    // Метод для отправки сообщений
    void send(const std::string& message);

    // Перегрузка оператора >> для приема данных
    SocketWrapper& operator>>(std::string& message);
    SocketWrapper& operator<<(std::string& message);

private:
    SOCKET clientSocket;
};
