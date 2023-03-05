window.addEventListener('load', () => {
    setTimeout(() => {
        

    const ratings = document.querySelectorAll('.rating'); /*Mass of ratings on page (mb)*/
    if (ratings.length > 0){
        initRatings();
    }

    function  initRatings() {
        let ratingActive, ratingValue;
        for (let index = 0; index <ratings.length; index++) {
            const rating = ratings[index];
            initRating(rating);
        }

        function initRating(rating) {
            initRatingVars(rating);
            setRatingActiveWidth(); /*Can change info if you have not number as 3.6 */
            if (rating.classList.contains('rating_set')) { /*If parent have class can set rating*/
                setRating(rating);
            }
        }

        function initRatingVars(rating) {
            ratingActive = rating.querySelector('.rating_active');
            ratingValue = rating.querySelector('.rating_value');
        }
        function setRatingActiveWidth(index = ratingValue.innerHTML) { /*ratindValue default number as 3.6 Value*/
            const ratingActiveWidth = index / 0.05;
            ratingActive.style.width = `${ratingActiveWidth}%`;
        }



        function setRating(rating) {
            let ratingItems = rating.querySelectorAll('.rating_item');
            for (let index = 0; index < ratingItems.length; index++) {
                const ratingItem = ratingItems[index];
                ratingItem.addEventListener("mouseenter", function(e) {
                    initRatingVars(rating);
                    setRatingActiveWidth(ratingItem.value);
                });
                ratingItem.addEventListener("mouseleave", function (e) {
                    setRatingActiveWidth(ratingValue.innerHTML); //Can change to 0 if u need to set rating 0 ( on reviews page)
                });
                ratingItem.addEventListener("click", function(e) {
                    initRatingVars(rating);
                    /*                if (rating.dataset.ajax) {
                                    //Send server
                                    //setRatingValue(ratingItem.value, rating);             //ajax !!!
                                }
                                else {*/
                    //show this mark
                    ratingValue.innerHTML = index; //+1 if 5 statrs
                    setRatingActiveWidth();
                    //}
                })
            }
        }



        /*    async function setRatingValue(value, rating) {
                if (!rating.classList.contains('rating_sending')) {
                    rating.classList.add('rating_sending');
                    let response = await fetch('rating.json', { // CHANGE TO MY PATH!!!!!!
                        method: 'GET',
                        body: JSON.stringify({
                            userRating: value
                        }),
                        headers: {
                            'content-type': 'application/json'
                        }
                    });
                    if (response.ok) {
                        const result = await response.json();
                        const newRating = result.newRating; // result.newRating from json from server
                        ratingValue.innerHTML = newRating;
        
                        rating.classList.remove('rating_sending');
                    } else {
                        alert("Ошибка"); //Smth write or do!!!! NESS VERY NESS!
                        rating.classList.remove('rating_sending');
                    }
                }
            }*/
    }

    },100)
})