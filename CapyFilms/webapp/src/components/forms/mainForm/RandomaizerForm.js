import React, { useState, useEffect } from "react";
import axios from "axios";
import { useAuth } from "../../AuthProvider";
import saveIcon from "../../../img/marks.svg";

const RandomizerForm = () => {
    const { token } = useAuth();
    const [randomFilm, setRandomFilm] = useState(null);
    const [genre, setGenre] = useState("");
    const [year, setYear] = useState("");
    const [director, setDirector] = useState("");
    const [actor, setActor] = useState("");
    const [loading, setLoading] = useState(false);

    const genres = ["", "Драма", "Семейный", "Ужасы", "Приключения", "Криминал"];

    // Выполняем запрос один раз при монтировании компонента, если токен доступен
    useEffect(() => {
        if (token) {
            fetchRandomFilm();
        }
    }, []); // Пустой массив зависимостей — срабатывает только при монтировании

    const fetchRandomFilm = async (body = {}) => {
        if (!token) return;

        setLoading(true);
        try {
            const url = "https://localhost:7294/api/v1/Cinema/random";
            const response = await axios.post(url, body, {
                headers: { Authorization: `Bearer ${token}` },
            });
            console.log("Ответ API:", response.data);
            setRandomFilm(response.data.data || null);
        } catch (error) {
            console.error("Ошибка при получении случайного фильма:", error.response?.data || error.message);
            alert(error.response?.data?.message || "Не удалось загрузить случайный фильм.");
        } finally {
            setLoading(false);
        }
    };

    const handleSaveFilm = (filmId) => {
        if (!token) return;

        const saveUrl = `https://localhost:7294/api/v1/Cinema/bookmarks?idCinema=${filmId}`;
        axios
            .post(saveUrl, {}, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((response) => {
                console.log(`Фильм ${filmId} добавлен в отложенные:`, response.data);
                alert("Фильм добавлен в отложенные!");
            })
            .catch((error) => {
                console.error("Ошибка при сохранении:", error);
                alert("Не удалось добавить фильм.");
            });
    };

    const handleStatusChange = (filmId, newStatus) => {
        if (!token) return;

        // Обновляем статус фильма через API
        const statusUrl = `https://localhost:7294/api/v1/Cinema/status?idCinema=${filmId}&status=${newStatus}`;
        axios
            .post(statusUrl, {}, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((response) => {
                console.log(`Статус фильма ${filmId} обновлён:`, response.data);
                // Обновляем локальное состояние
                setRandomFilm((prevFilm) => ({
                    ...prevFilm,
                    status: newStatus,
                }));
            })
            .catch((error) => {
                console.error("Ошибка при обновлении статуса:", error);
                alert("Не удалось обновить статус фильма.");
            });
    };

    const getStatusText = (status) => {
        return status === true ? "Просмотрено" : "Не просмотрено";
    };

    const handleSearch = () => {
        const body = {
            genre: genre || null,
            year: year || null,
            producer: director || null,
            actor: actor || null,
        };
        fetchRandomFilm(body);
    };

    return (
        <div className="main-content main-content--top">
            <div className="header_frame">
                <input
                    type="text"
                    className="search_cinema"
                    placeholder="Поиск фильма..."
                />
            </div>
            <div className="content_frame">
                <h2 className="randomizer-title">Подобрать фильм:</h2>
                <div className="randomizer-container">
                    <div className="randomizer-content">
                        <div className="randomizer-filters">
                            <div className="filter-group">
                                <label className="filter-label">Жанр:</label>
                                <select
                                    className="randomizer-select"
                                    value={genre}
                                    onChange={(e) => setGenre(e.target.value)}
                                >
                                    {genres.map((g, index) => (
                                        <option key={index} value={g}>
                                            {g || "Жанр"}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="filter-group">
                                <label className="filter-label">Год:</label>
                                <input
                                    type="text"
                                    className="randomizer-input"
                                    value={year}
                                    onChange={(e) => setYear(e.target.value)}
                                    placeholder="Введите год"
                                />
                            </div>
                            <div className="filter-group">
                                <label className="filter-label">Режиссер:</label>
                                <input
                                    type="text"
                                    className="randomizer-input"
                                    value={director}
                                    onChange={(e) => setDirector(e.target.value)}
                                    placeholder="Введите режиссера"
                                />
                            </div>
                            <div className="filter-group">
                                <label className="filter-label">Актер/Актриса:</label>
                                <input
                                    type="text"
                                    className="randomizer-input"
                                    value={actor}
                                    onChange={(e) => setActor(e.target.value)}
                                    placeholder="Введите актера/актрису"
                                />
                            </div>
                            <button className="randomize-button" onClick={handleSearch}>
                                Искать
                            </button>
                        </div>
                        <div className="divider-line"></div>
                        <div className="random-film-section">
                            {loading ? (
                                <div className="loading">Загрузка...</div>
                            ) : randomFilm && (
                                <div className="random-film-card">
                                    <div className="poster-wrapper">
                                        {randomFilm.posterUrl ? (
                                            <img
                                                className="img_cinema"
                                                alt={randomFilm.name}
                                                src={randomFilm.posterUrl}
                                            />
                                        ) : (
                                            <div className="poster-placeholder"></div>
                                        )}
                                        {randomFilm.posterUrl && (
                                            <button
                                                className="save-icon"
                                                onClick={() => handleSaveFilm(randomFilm.id)}
                                                title="Добавить в отложенные"
                                            >
                                                <img
                                                    src={saveIcon}
                                                    alt="Save to deferred"
                                                    className="save-icon-img"
                                                />
                                            </button>
                                        )}
                                    </div>
                                    <h1 className="name_cinema">{randomFilm.name || "Название недоступно"}</h1>
                                    <div className="status-section">
                                        <label>
                                            <input
                                                type="checkbox"
                                                checked={randomFilm.status === true}
                                                onChange={(e) =>
                                                    handleStatusChange(randomFilm.id, e.target.checked)
                                                }
                                            />
                                            <span className="status_cinema">
                                                {getStatusText(randomFilm.status)}
                                            </span>
                                        </label>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RandomizerForm;