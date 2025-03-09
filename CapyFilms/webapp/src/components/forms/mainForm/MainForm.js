import React, { useState } from "react";
import Header from "./HeaderInput";
import Content from "./ContentForm";
import Nav from "./NavForm";
import PersonalForm from "./PersonalAccount";
import RandomizerForm from "./RandomaizerForm"; 

function Main() {
    const [searchQuery, setSearchQuery] = useState("");
    const [searchSubmitted, setSearchSubmitted] = useState(false);
    const [activeView, setActiveView] = useState("home");
    const [deferredFilms, setDeferredFilms] = useState([]); // Состояние для отложенных фильмов
    const [watchedFilms, setWatchedFilms] = useState([]); // Состояние для просмотренных фильмов
    const [activeFilmType, setActiveFilmType] = useState(null); // Тип активных фильмов: 'deferred' или 'watched'

    // Функция для обновления состояния поиска
    const handleSearchChange = (query) => {
        setSearchQuery(query);
    };

    // Функция для обработки поиска
    const handleSearchSubmit = () => {
        setSearchSubmitted(true);
        console.log("Searching for:", searchQuery);
    };

    const handleViewChange = (view) => {
        if (view === "home") {
            setDeferredFilms([]); // Сбрасываем отложенные фильмы
            setWatchedFilms([]); // Сбрасываем просмотренные фильмы
            setActiveFilmType(null); // Сбрасываем активный тип фильмов
            setSearchSubmitted(false); // Сбрасываем поиск
        }
        setActiveView(view);
    };

    // Функция для получения фильмов из PersonalForm
    const handleFetchFilms = (films, filmType) => {
        if (filmType === "deferred") {
            setDeferredFilms(films);
            setWatchedFilms([]); // Очищаем просмотренные фильмы
            setActiveFilmType("deferred");
        } else if (filmType === "watched") {
            setWatchedFilms(films);
            setDeferredFilms([]); // Очищаем отложенные фильмы
            setActiveFilmType("watched");
        }
        setActiveView("home"); // Возвращаемся к виду "home" для отображения фильмов
        setSearchSubmitted(false); // Сбрасываем поиск
    };

    // Логика для определения, какие фильмы отображать
    const filmsToDisplay =
        activeFilmType === "watched"
            ? watchedFilms
            : activeFilmType === "deferred"
            ? deferredFilms
            : [];

    return (
        <div className="main">
            <div className="main-frame">
                <Nav onViewChange={handleViewChange} activeView={activeView} />
                <div
                    className={`content-wrapper ${
                        activeView === "personal"
                            ? "content-wrapper--centered"
                            : "content-wrapper--top"
                    }`}
                >
                    {activeView === "personal" ? (
                        <PersonalForm onFetchFilms={handleFetchFilms} />
                    ) : activeView === "random" ? (
                        <RandomizerForm /> // Отображаем RandomizerForm при выборе "random"
                    ) : (
                        <>
                            <Header
                                searchQuery={searchQuery}
                                onSearchChange={handleSearchChange}
                                onSearchSubmit={handleSearchSubmit}
                            />
                            <Content
                                searchQuery={searchQuery}
                                searchSubmitted={searchSubmitted}
                                deferredFilms={deferredFilms}
                                watchedFilms={watchedFilms}
                                activeFilmType={activeFilmType}
                                showGenre={
                                    activeView === "home" &&
                                    !activeFilmType
                                }
                            />
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}

export default Main;