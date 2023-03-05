async function showRating() {
    const ratings = document.querySelectorAll('.rating'); /*Mass of ratings on page (mb)*/
    if (ratings.length > 0){
        initRatings();
    }

    function initRatings() {
        let ratingActive, ratingValue;
        for (let index = 0; index < ratings.length; index++) {
            const rating = ratings[index];
            initRating(rating);
        }

        function initRating(rating) {
            initRatingVars(rating);
            setRatingActiveWidth();
        }

        function initRatingVars(rating) {
            ratingActive = rating.querySelector('.rating_active');
            ratingValue = rating.querySelector('.rating_value');
        }

        function setRatingActiveWidth(index = ratingValue.innerHTML) {
            const ratingActiveWidth = index / 0.05;
            ratingActive.style.width = `${ratingActiveWidth}%`;
        }
    }
}