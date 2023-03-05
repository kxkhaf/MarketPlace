async function addReviewItem(id, name, information, rating, realId, price) {
    let reviewItem = document.getElementById("reviewItemCollector");
    let item = document.createElement("div");
    let starRating = makeStars(rating);
    item.innerHTML = `
    <div class="reviewItem" id="${id}review" style="top: calc(${getCount()} * (1vmin + 1vmax));">
        <strong title="${name}" style="
        flex-direction: column;
        text-align: center;
        white-space: nowrap;
        top: 2%;
        -webkit-text-stroke: calc(0.01 * (1vw + 2vh)) #ddcd02;
        font-family: Luminari, fantasy;
        text-overflow: ellipsis;
        position: absolute;
        font-size: calc(2 * (1vmin + 1vmax));
        ">${name}
            <div class="form_item">
                <div class="rating">
                    <div class="rating_body">
                        <div class="rating_active" style="width: ${rating * 20}%"></div>
                        <div title="${starRating}" class="rating_items">
                            <input type="radio" class="rating_item" value="0" name="rating">
                            <input type="radio" class="rating_item" value="1" name="rating">
                            <input type="radio" class="rating_item" value="2" name="rating">
                            <input type="radio" class="rating_item" value="3" name="rating">
                            <input type="radio" class="rating_item" value="4" name="rating">
                            <input type="radio" class="rating_item" value="5" name="rating">
                        </div>
                    </div>
                    <div title="Рейтинг - ${rating}" class="rating_value">${rating}</div>
                </div>
            </div>                                             
                <textarea title="${information}" class="reviewInfo" maxlength="250" readonly>${information}</textarea>
        </strong>
        <div title="${price}" class="reviewPrice">${price}<text style="font-size: calc((1vmin/ 2 + 1vmax)); margin-top: font-size: calc((1vw/ 2 + 1vh/ 4))">⚡</text></div>
    </div>
    `;
    reviewsId.push(id);
    reviewsOnPage.push(new Review(id, name, information, rating, realId, price));
    reviewItem.appendChild(item);
}

async function addReviewItemCanEdit(id, name, information, rating, realId, price) {
    let reviewItem = document.getElementById("reviewItemCollector");
    let item = document.createElement("div");
    let starRating = makeStars(rating);
    item.innerHTML = `
    <div class="reviewItem" id="${id}review" style="top: calc(${getCount()} * (1vmin + 1vmax));">
        <strong title="${name}" style="
        flex-direction: column;
        text-align: center;
        white-space: nowrap;
        top: 2%;
        -webkit-text-stroke: calc(0.01 * (1vw + 2vh)) #ddcd02;
        font-family: Luminari, fantasy;
        text-overflow: ellipsis;
        position: absolute;
        font-size: calc(2 * (1vmin + 1vmax));
        ">${name}
            <div class="form_item">
                <div class="rating">
                    <div class="rating_body">
                        <div class="rating_active" style="width: ${rating * 20}%"></div>
                        <div title="${starRating}" class="rating_items">
                            <input type="radio" class="rating_item" value="0" name="rating">
                            <input type="radio" class="rating_item" value="1" name="rating">
                            <input type="radio" class="rating_item" value="2" name="rating">
                            <input type="radio" class="rating_item" value="3" name="rating">
                            <input type="radio" class="rating_item" value="4" name="rating">
                            <input type="radio" class="rating_item" value="5" name="rating">
                        </div>
                    </div>
                    <div title="Рейтинг - ${rating}" class="rating_value">${rating}</div>
                </div>
            </div>                                             
                <textarea title="${information}" class="reviewInfo" id="reviewInfo" maxlength="250" readonly>${information}</textarea>
                <div style="position: relative; top: calc(6 * (1vmin - 1vmax))">
                    <button title="Delete review." class="deleteBtn" onclick="deleteReview(${id}, this.parentElement.parentElement, ${rating}, ${realId})">-</button>
                </div>
        </strong>
        <div title="${price}" class="reviewPrice">${price}<text style="font-size: calc((1vmin/ 2 + 1vmax)); margin-top: font-size: calc((1vw/ 2 + 1vh/ 4))">⚡</text></div>
    </div>
    `;
    reviewsId.push(id);
    reviewsOnPage.push(new Review(id, name, information, rating, realId, price));
    reviewItem.appendChild(item);
}

async function addReviewItemCanWrite(id, name, information, rating, realId, price) {
    let reviewItem = document.getElementById("reviewItemCollector");
    let item = document.createElement("div");
    let starRating = makeStars(rating);
    item.innerHTML = `
    <div class="reviewItem" id="${id}review" style="top: calc(${getCount()} * (1vmin + 1vmax));">
        <strong title="${name}" style="
        flex-direction: column;
        text-align: center;
        white-space: nowrap;
        top: 2%;
        -webkit-text-stroke: calc(0.01 * (1vw + 2vh)) #ddcd02;
        font-family: Luminari, fantasy;
        text-overflow: ellipsis;
        position: absolute;
        font-size: calc(2 * (1vmin + 1vmax));
        ">${name}
            <div class="form_item">
                <div class="rating rating_set">
                    <div class="rating_body">
                        <div class="rating_active" style="width: ${rating * 20}%"></div>
                        <div title="${starRating}" class="rating_items">
                            <input type="radio" class="rating_item" value="0" name="rating">
                            <input type="radio" class="rating_item" value="1" name="rating">
                            <input type="radio" class="rating_item" value="2" name="rating">
                            <input type="radio" class="rating_item" value="3" name="rating">
                            <input type="radio" class="rating_item" value="4" name="rating">
                            <input type="radio" class="rating_item" value="5" name="rating">
                        </div>
                    </div>
                    <div title="Рейтинг - ${rating}" class="rating_value">${rating}</div>
                </div>
            </div>                                         
                <label for="reviewInfo"/>
                <textarea typeof="input" title="${information}" id="reviewInfo" class="reviewInfo" name="reviewInfo" maxlength="250" placeholder="Your mind..."></textarea>
                <div style="position: relative; top: calc(6 * (1vmin - 1vmax))">
                    <button title="Add review." class="addBtn" onclick="addReview(Number(${realId}), this.parentElement.parentElement)">+</button>
                </div>
        </strong>
        <div title="${price}" class="reviewPrice">${price}<text style="font-size: calc((1vmin/ 2 + 1vmax)); margin-top: font-size: calc((1vw/ 2 + 1vh/ 4))">⚡</text></div>
    </div>
    `;
/*        let area = this.parentElement.parentElement.querySelector('#reviewInfo');
        if (area.addEventListener) {
            area.addEventListener('input', function() {
                area.value = 
            }, false);
        } else if (area.attachEvent) {
            area.attachEvent('onpropertychange', function() {
                // IE-specific event handling code
            });
        }
    })*/
    reviewsId.push(id);
    reviewsOnPage.push(new Review(id, name, information, rating, realId, price));
    reviewItem.appendChild(item);
}

async function deleteReview(id, node, rating, realId) {
    console.log(rating);
    console.log(node.querySelector("#reviewInfo").value);
    let information = node.querySelector("#reviewInfo").value;
    await fetch("/deleteReview", {method: "POST", body: JSON.stringify(new function() {
        this.Message = information;
        this.Rating = Number(rating);
        this.ProductId = Number(realId);
        })})
    setTimeout(() => {
        location.reload();
    }, 5 * 1000)
    information = node.querySelector("#reviewInfo").value = "Deleting...";
    //await removeReview(Number(id));
}

let Name = undefined;
let Price = undefined;
let productId = undefined;
let productRating = 0;
let isSending = false;
let reviewsOnPage = [];
let reviewsOnDB = [];
let reviewsId = [];
let count = -14;

let balanceElement;
let balance;

async function getProductById() {
    let result = await (await fetch('/getUserProductById', {method: "POST", body: JSON.stringify(productId)})).text();
    let product = JSON.parse(result);
    Name = product.Name;
    Price = product.Price
    console.log(product)
}

async function getReviewsFromDB() {
    await getProductId();
    await getProductById();
    await addReviewItemCanWrite(reviewsId.length, Name, null, productRating, productId, Price);
    await getReviewsFromDBCanEdit();
    await getReviewsFromDBNotEdit();
}

async function getReviewsFromDBNotEdit() {
    let result = await (await fetch('/getUserReviews', {method: "POST", body: JSON.stringify(productId)})).text();
    reviewsOnDB = JSON.parse(result);
    console.log(result);
    for (let i = 0; i < reviewsOnDB.length; i++) {
        await addReviewItemWithIndex(reviewsOnDB[i].Name, reviewsOnDB[i].Message, reviewsOnDB[i].Rating, reviewsOnDB[i].Id, reviewsOnDB[i].Price);
    }
}

async function getReviewsFromDBCanEdit() {
    let result = await (await fetch('/getUserReviewsCanEdit', {method: "POST", body: JSON.stringify(productId)})).text();
    reviewsOnDB = JSON.parse(result);
    console.log(result);
    for (let i = 0; i < reviewsOnDB.length; i++) {
        await addReviewItemWithIndexCanEdit(reviewsOnDB[i].Name, reviewsOnDB[i].Message, reviewsOnDB[i].Rating, reviewsOnDB[i].Id, reviewsOnDB[i].Price);
    }
    setTimeout(() => {
        balanceElement = document.querySelector('#UserBalance');
        if (balanceElement != null){
            balance = Number(balanceElement.textContent.
            replace('Balance: ', '').replace('⚡', ''));
            console.log(balance);
        }
    }, 500)
}

function getCount() {
    count += 21;
    return count;
}

async function addReviewItemWithIndex(name, information, rating, realId, price) {
    await addReviewItem(reviewsId.length, name, information, rating, realId, price);
}

async function addReviewItemWithIndexCanEdit(name, information, rating, realId, price) {
    await addReviewItemCanEdit(reviewsId.length, name, information, rating, realId, price);
}

async function getProductId() {
    let cookie = document.cookie;
    let data = cookie.split(";");
    for (let i = 0; i < data.length; i++) {
        let info = data[i].split("=");
        if (JSON.stringify(info[0]).replace(" ", "") === JSON.stringify("id")) {
            productId = info[1];
        }
    }
    return productId;
}
async function getProductData() {
    let cookie = document.cookie;
    let data = cookie.split(";");
    for (let i = 0; i < data.length; i++) {
        let info = data[i].split("=");
        console.log(data);
        if (JSON.stringify(info[0]).replace(" ", "") === JSON.stringify("rating")) {
            productRating = info[1];
        }
        if (JSON.stringify(info[0]).replace(" ", "") === JSON.stringify("id")) {
            productId = info[1];
        }
    }
}
function CookieData(name, path, express) {
    this.Name = name;
    this.Path = path;
    this.Express = express;
}

async function getReviews() {
    console.log(window.location)
}

function Reviews(id, name, information, rating, count, realId, price){
    this.Id = id;
    this.Name = name;
    this.Information = information;
    this.Rating = rating;
    this.Count = count;
    this.RealId = realId;
    this.Price = price;
}


function Review(id, name, information, rating, count, realId, price){
    this.Id = id;
    this.Name = name;
    this.Message = information;
    this.Rating = rating;
    this.RealId = realId;
    this.Price = price;
}
                        
let timeout = 1000;
async function addReview(reviewId, reviewElement) {
    try {
        let messageInfo = reviewElement.parentElement.querySelector("#reviewInfo");
        productRating = reviewElement.parentElement.parentElement.querySelector(".rating_value").innerText;
        console.log(productRating)
        if (sendingUserReviews.length > 3)
        {
            sendingUserReviews = [];
            await reloadPage();
        }
        await AddSendingData(messageInfo);
    }
    catch (exception) {
        //reloadPage()
    }
}

function SendingReview(id ,reviewerId, productId, rating, message) {
    this.Id =-1;
    this.ReviewerId = -1;
    this.ProductId = productId;
    this.Rating = rating;
    this.Message = message;
}

let sendingUserReviews = [];  
async function AddSendingData(message) {
    productRating = Math.ceil(Number(productRating));
    console.log(productRating);
    sendingUserReviews.push(new SendingReview(-1, -1, Number(productId), productRating, message.value));
    if (!isSending){
        SendDataToDB();
    }
    let userName = document.querySelector("#userName");
/*    message.value = "Loading...";
    setTimeout(() => {location.reload()}, 6 * 1000)*/
    await addReviewItemCanEdit(reviewsId.length, userName.innerText, message.value, Number(productRating), Number(productId), Price);
    setTimeout(() => {
        if (JSON.stringify(message.value) === JSON.stringify("Loading...")){
        message.value = null;
        }
    }, 5 * 1000)
    message.value = "Loading...";
}
function SendDataToDB() {
    console.log(1)
    isSending = true;
    window.setTimeout(() => {
        let response = fetch("/addReviews",
            { method: "POST", body: JSON.stringify(sendingUserReviews)});
        console.log(sendingUserReviews)
        sendingUserReviews = [];
        response.then(response => {if (!response.ok){
            location.reload();
        }}).catch(() =>  {window.clearTimeout(); reloadPage();});
        isSending = false;
    }, 5000)
}

function reloadPage() {
    location.reload();
}


function UserReview(userId, productId, reviewRating){
    this.UserId = userId;
    this.ProductId = productId;
    this.ReviewRating = reviewRating;
}

function makeStars(rating) {
    let starRating ="";
    if (rating < 1){
        starRating = "No rating!";
    }
    for (let i = 0; i < Math.floor(rating); i++){
        starRating += "★";
    }
    return starRating;
}

async function removeReview(id) {
    if (id < reviewsId.length) {
        document.getElementById(String(id) + "review").remove();
        console.log(reviewsOnDB);
        await moveUpElements(id);
        count -= 21;
        reviewsId.pop();
        console.log(reviewsOnPage)
        for (let i = id; i < reviewsOnPage.length - 1; i++){
            reviewsOnPage[i] = reviewsOnPage[i + 1];
            reviewsOnPage[i].id--;
        }
        reviewsOnDB.pop();
        console.log(reviewsOnPage)
        console.log(reviewsOnDB);
    }
}

async function moveUpElements(deletedId) {
    for (let i = deletedId + 1; i < reviewsId.length; i++) {
        let review = document.getElementById(i + "review");
        review.setAttribute("id",
            (Number(review.id.replace("review", "")) - 1) + "review");
        review.setAttribute("style", "top:" + 'calc(' + Number(review.id.replace("review", "") * 21 + 7) + ' * (1vmin + 1vmax))');
    }
}

