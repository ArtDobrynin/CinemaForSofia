import React, { useState, useEffect } from "react";
import axios from "axios";
import { jwtDecode } from "jwt-decode";

const PersonalForm = ({ onFetchFilms }) => {
    const [user, setUser] = useState("");
    const [email, setEmail] = useState("");

    useEffect(() => {
        const fetchUserData = () => {
            try {
                const token = localStorage.getItem("authToken");

                if (token) {
                    const decodedToken = jwtDecode(token);

                    const userName = decodedToken.unique_name || "Не указано";
                    const userEmail = decodedToken.email || "Не указано";

                    setUser(userName);
                    setEmail(userEmail);
                } else {
                    console.log("Токен не найден");
                    setUser("Не авторизован");
                    setEmail("Не авторизован");
                }
            } catch (error) {
                console.error("Ошибка декодирования токена:", error);
                setUser("Ошибка");
                setEmail("Ошибка");
            }
        };

        fetchUserData();
    }, []);

    // Обработчик клика на кнопку для получения списка отложенных фильмов
    const handleFetchDeferredFilms = async () => {
        try {
            const token = localStorage.getItem("authToken");
            if (!token) {
                alert("Токен не найден. Пожалуйста, авторизуйтесь.");
                return;
            }

            const response = await axios.get(
                "https://localhost:7294/api/v1/Cinema/bookmarksList",
                {
                    headers: { Authorization: `Bearer ${token}` },
                }
            );

            const deferredFilms = response.data.data || [];
            onFetchFilms(deferredFilms, "deferred");
        } catch (error) {
            console.error("Ошибка при получении отложенных фильмов:", error);
            alert("Не удалось загрузить отложенные фильмы.");
        }
    };

    // Обработчик для получения списка просмотренных фильмов
    const handleFetchWatchedFilms = async () => {
        try {
            const token = localStorage.getItem("authToken");
            if (!token) {
                alert("Токен не найден. Пожалуйста, авторизуйтесь.");
                return;
            }

            const response = await axios.get(
                "https://localhost:7294/api/v1/Cinema/watchedList",
                {
                    headers: { Authorization: `Bearer ${token}` },
                }
            );

            const watchedFilms = response.data.data || [];
            onFetchFilms(watchedFilms, "watched");
        } catch (error) {
            console.error("Ошибка при получении просмотренных фильмов:", error);
            alert("Не удалось загрузить просмотренные фильмы.");
        }
    };

    return (
        <div className="form-container">
            <h2>Личная страница</h2>
            <div className="form-group">
                <label>Пользователь:</label>
                <span className="form-text">{user}</span>
            </div>
            <div className="form-group">
                <label>Почта:</label>
                <span className="form-text">{email}</span>
            </div>
            <div className="button-group">
                <button
                    className="submit-button"
                    onClick={handleFetchDeferredFilms}
                >
                    Отложенные фильмы
                </button>
                <button
                    className="submit-button"
                    onClick={handleFetchWatchedFilms}
                >
                    Просмотренные фильмы
                </button>
            </div>
        </div>
    );
};

export default PersonalForm;