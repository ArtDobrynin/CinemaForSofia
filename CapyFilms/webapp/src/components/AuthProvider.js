import React, { createContext, useState, useContext, useEffect } from 'react';

// Создаем контекст для авторизации
const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

// Компонент провайдер, который будет хранить токен
export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem('authToken') || null);

  // Сохраняем токен в localStorage, если он есть
  useEffect(() => {
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  }, [token]);

  const login = (newToken) => setToken(newToken); // Вход
  const logout = () => setToken(null); // Выход

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};