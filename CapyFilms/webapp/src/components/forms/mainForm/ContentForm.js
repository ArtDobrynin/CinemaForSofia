import React, { useState } from "react";
import Genre from "./GenreForm";
import CinemaList from "./CinemaForm";

function Content({
    searchQuery,
    searchSubmitted,
    deferredFilms,
    watchedFilms,
    activeFilmType,
    showGenre,
}) {
    const [selectedGenre, setSelectedGenre] = useState("Новинки");

    const handleGenreChange = (genre) => {
        setSelectedGenre(genre);
    };

    const filmsToDisplay =
        activeFilmType === "watched"
            ? watchedFilms
            : activeFilmType === "deferred"
            ? deferredFilms
            : [];

    return (
        <div className="content_frame">
            {showGenre && <Genre onGenreChange={handleGenreChange} />}
            <CinemaList
                selectedGenre={selectedGenre}
                searchQuery={searchQuery}
                searchSubmitted={searchSubmitted}
                films={filmsToDisplay}
                activeFilmType={activeFilmType} // Передаем activeFilmType в CinemaList
            />
        </div>
    );
}

export default Content;