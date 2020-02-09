﻿$(document).ready(function () {
    function search() {
        var url = '/date/' + $('.cinema-year').val() + '/' + $('.cinema-month').val() + '/' + $('.cinema-day').val();
        var cinemaId = $('#CinemaID').val();
        var genreId = $('#GenreID').val();
        var movieName = $('.cinema-movie-name').val();

        cinemaId = genreId != '' && cinemaId == '' ? 0 : cinemaId;
        if (cinemaId != '') {
            url += '/' + cinemaId;
        }

        if (genreId != '') {
            if (cinemaId == '') {
                url += '/0';
            }
            url += '/' + genreId;
        }

        if (movieName != "") {
            url += '?searchString=' + movieName;
        }

        window.location.href = url;
    }

    $('.cinema select').change(function () {
        search();
    });

    $('.search').click(function () {
        search();
    });

    var moviesSlider = $('.movies').lightSlider({
        pager: false,
        autoWidth: true,
        responsive: [
            {
                breakpoint: 800,
                settings: {
                    item: 4
                }
            },
            {
                breakpoint: 480,
                settings: {
                    item: 1,
                    autoWidth: false
                }
            },
        ],
    });

    $('.movies-slider-next').click(function () {
        moviesSlider.goToNextSlide();
    });

    $('.movies-slider-prev').click(function () {
        moviesSlider.goToPrevSlide();
    });
});