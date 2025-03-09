import React, { useState, useEffect } from "react";
import axios from "axios";
import { useAuth } from "../../AuthProvider";
import saveIcon from "../../../img/marks.svg";
import watchedIcon from "../../../img/check_mark.svg";

const CinemaList = ({
    selectedGenre,
    searchQuery,
    searchSubmitted,
    films,
    activeFilmType, // Проп для определения типа списка
}) => {
    const [filmList, setFilmList] = useState([]);
    const { token, logout } = useAuth();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!token) return;

        setLoading(true);

        let url = "";
        if (searchQuery && searchSubmitted) {
            url = `https://localhost:7294/api/v1/Cinema/search?stringSearch=${searchQuery}`;
        } else if (selectedGenre && selectedGenre !== "Новинки") {
            url = `https://localhost:7294/api/v1/Cinema/genre?genreName=${selectedGenre}`;
        } else if (films && films.length > 0) {
            setFilmList(films);
            setLoading(false);
            return;
        } else {
            url = "https://localhost:7294/api/v1/Cinema";
        }

        setFilmList([]);

        if (!url) return;

        axios
            .get(url, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((response) => {
                setFilmList(response.data.data || []);
                setLoading(false);
            })
            .catch((error) => {
                if (error.response && error.response.status === 401) {
                    localStorage.removeItem("authToken");
                    logout();
                    alert("Session expired. Please log in again.");
                }
                setLoading(false);
            });
    }, [token, selectedGenre, searchQuery, searchSubmitted, films]);

    const handleSaveFilm = (filmId) => {
        const saveUrl = `https://localhost:7294/api/v1/Cinema/bookmarks?idCinema=${filmId}`;
        axios
            .post(saveUrl, {}, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((response) => {
                console.log(`Фильм ${filmId} добавлен в отложенные:`, response.data);
                alert("Фильм добавлен в отложенные!");
                if (response.data && response.data.status !== undefined) {
                    setFilmList(
                        filmList.map((film) =>
                            film.id === filmId ? { ...film, status: response.data.status } : film
                        )
                    );
                }
            })
            .catch((error) => {
                console.error("Ошибка при сохранении:", error);
                alert("Не удалось добавить фильм.");
            });
    };

    const handleMarkWatched = (filmId) => {
        const currentFilm = filmList.find((film) => film.id === filmId);
        if (!currentFilm) return;

        const newStatus = !currentFilm.status; // Переключаем статус: true -> false, false -> true
        const watchedUrl = `https://localhost:7294/api/v1/Cinema/watched?idCinema=${filmId}`;

        axios
            .post(
                watchedUrl,
                { status: newStatus },
                {
                    headers: { Authorization: `Bearer ${token}` },
                }
            )
            .then((response) => {
                console.log(`Фильм ${filmId} обновлен:`, response.data);
                alert(newStatus ? "Фильм отмечен как просмотренный!" : "Статус просмотра снят!");

                // Если текущий список — отложенные и фильм стал просмотренным, удаляем его
                if (activeFilmType === "deferred" && newStatus) {
                    setFilmList(filmList.filter((film) => film.id !== filmId));
                }
                // Если текущий список — просмотренные и статус снят, удаляем его
                else if (activeFilmType === "watched" && !newStatus) {
                    setFilmList(filmList.filter((film) => film.id !== filmId));
                }
                // В остальных случаях обновляем статус
                else {
                    setFilmList(
                        filmList.map((film) =>
                            film.id === filmId ? { ...film, status: newStatus } : film
                        )
                    );
                }
            })
            .catch((error) => {
                console.error("Ошибка при обновлении статуса:", error);
                alert("Не удалось обновить статус фильма.");
                setFilmList(
                    filmList.map((film) =>
                        film.id === filmId ? { ...film, status: currentFilm.status } : film
                    )
                );
            });
    };

    const getStatusText = (status) => {
        if (status === null || status === undefined) return "Статус неизвестен";
        return status ? "Просмотрено" : "Не просмотрено";
    };

    return (
        <div className="cinema_panel">
            <ul className="cinema_list">
                {loading ? (
                    <p>Загрузка...</p>
                ) : filmList.length > 0 ? (
                    filmList.map((film, index) => (
                        <li key={index} className="cinema_element">
                            <div className="cinema_card">
                                <div className="poster-wrapper">
                                    {film.posterUrl ? (
                                        <img
                                            className="img_cinema"
                                            alt={film.name}
                                            src={film.posterUrl}
                                        />
                                    ) : (
                                        <div className="no-poster-placeholder">
                                            <p>Изображение недоступно</p>
                                        </div>
                                    )}
                                    <button
                                        className="save-icon"
                                        onClick={() => handleSaveFilm(film.id)}
                                        title="Добавить в отложенные"
                                    >
                                        <img
                                            src={saveIcon}
                                            alt="Save to deferred"
                                            className="save-icon-img"
                                        />
                                    </button>
                                    <button
                                        className="watched-icon"
                                        onClick={() => handleMarkWatched(film.id)}
                                        title="Отметить как просмотренное/снять"
                                    >
                                        <img
                                            src={watchedIcon}
                                            alt="Mark as watched"
                                            className="watched-icon-img"
                                        />
                                    </button>
                                </div>
                                <h1 className="name_cinema">
                                    {film.name || "Название недоступно"}
                                </h1>
                                <h3
                                    className="status_cinema"
                                    style={{ color: film.status ? "#28a745" : "#749BC1B2" }}
                                >
                                    {getStatusText(film.status)}
                                </h3>
                            </div>
                        </li>
                    ))
                ) : (
                    <p>Нет доступных фильмов</p>
                )}
            </ul>
        </div>
    );
};

export default CinemaList;