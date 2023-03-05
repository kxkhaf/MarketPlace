async function addProductItem(id, name, information, rating, count, realId, price) {
    let productItem = document.getElementById("productItemCollector");
    let item = document.createElement("div");
    /*    item.setAttribute('id', "productItem");*/
    //<b title="${rating}" class="productRating" style="display: flex; flex-direction: column; text-align: left">${rating}
    let starRating = makeStars(rating);
    //calc(${getCount()} * (3vh / 2 + 1vmax - 1vmin));
    item.innerHTML = `
    <div class="productItem" id="${id}product" style="top: calc(${getCount()} * (1vmin + 1vmax));">
        <strong title="${name}" style="
        flex-direction: column;
        text-align: center;
        white-space: nowrap;
        top: 2%;
        -webkit-text-stroke: calc(0.01 * (1vw + 2vh)) #ddcd02;
        font-family: Luminari, fantasy;
        /* overflow: hidden; */
        text-overflow: ellipsis;
        position: absolute;
        font-size: calc(2 * (1vmin + 1vmax));
/*        text-outline: #ff6a00 calc(0.0001 * (1vmin + 1vmax));*/
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
                <textarea title="${information}" class="productInfo" maxlength="250" readonly>${information}</textarea>
                <div style="position: relative; top: calc(6 * (1vmin - 1vmax))">
                    <button title="Delete from cart." class="deleteBtn" onclick="changeProductCount(-1, Number(this.parentElement.parentElement.parentElement.id.replace('product', '')), ${realId})">-</button>
                    <button title="Add into cart." class="addBtn" onclick="changeProductCount(1, Number(this.parentElement.parentElement.parentElement.id.replace('product', '')), ${realId})">+</button>
                    <form action="/reviews" onsubmit="event.preventDefault()">
                        <button title="Make a review to this product." class="makeReview" onclick="getProductReviews(${realId}, this.parentElement.parentElement.parentElement)">
                            <img class="btnImg" src="/pictures/message.png" alt="reviewImage" style="pointer-events: none; height: 75%; width: auto;"/>
                        </button>
                    </form>
                </div>
        </strong>
        <div title="${count}" class="productCount">${count}</div>
        <div title="${price}" class="productPrice">${price}<text style=" font-size: calc((1vmin/ 2 + 1vmax)); margin-top: font-size: calc((1vw/ 2 + 1vh/ 4))">⚡</text></div>   
        </div>
    `;
    /*    <div class="productItem" id="classNameproduct" style="top: 35vh; left: 30vw">
            <strong style="display: flex; flex-direction: column;">${name}
                <b class="productRating" stclassNamedisplay: flex; flex-direction: column; text-align: left">{rating}</b>
            <label>
                <button class="deleteBtn">-
                </butclassName
                <button class="addBtn">+</buttonclassName         <textarea class="productInfo" maxlclassName="250" readonlymaxLengthation}<readOnlya>
        </label>
    </strong>
    </div>*/

    /*    <div class="productItem" id="classNameproduct" style="top: ${getCount()}vh; left: 30vw">
            <strong style="display: flex; flex-direction: column">${name}
                <label>
                <textarea class="productInfo" maxlength="250" readonly>${information}</textarea>
                    <readOnlya>
                </label>
            </strong>
        </div>*/
    productsId.push(id);
    productsOnPage.push(new Product(id, name, information, rating, count, realId, price));
    productItem.appendChild(item);
}

let isSending = false;
var productsOnPage = [];
var productsId = [];
var productsOnDB = [];
var count = -14;

let balanceElement;
let balance;

async function setCookie(name, value, path, time) {
    let date = new Date(Date.now() + time * 1e3);
    date = date.toUTCString();
    document.cookie = name + "=" + value + ";path=" + path + "; expires=" + date;
}

async function setReviewProductIdAsync(value) {
    await setCookie("id", value, "/", 30);
}

function setReviewCookie(id, rating)
{
    setReviewProductId(id);
    setReviewRating(rating);
}
function setReviewProductId(value) {
    setCookie("id", value, "/", 5 * 60);
}

function setReviewRating(value) {
    setCookie("rating", value, "/", 5 * 60);
}

async function getProductReviews(id, ratingElem) {
    let rating = 0;
    rating = Number(ratingElem.querySelector(".rating_value").innerText);
    setReviewCookie(id, rating);
    location.href = '/reviews';
    //let result = await (await fetch('/reviews', { method: "POST", headers: {"id": id}})).text();
/*    productsOnDB = JSON.parse(result);
    for (let i = 0; i < productsOnDB.length; i++) {
        await addProductItemWithIndex(productsOnDB[i].Name, productsOnDB[i].Information, productsOnDB[i].Rating, productsOnDB[i].Count, productsOnDB[i].Id, productsOnDB[i].Price);
    }
    setTimeout(() => {
        balanceElement = document.querySelector('#UserBalance');
        if (balanceElement != null){
            balance = Number(balanceElement.textContent.
            replace('Balance: ', '').replace('⚡', ''));
        }
    }, 500)
    await showRatingAfterLoad();*/
}

/*let element = document.getElementById(id + "product");
let countElement = element.querySelector('.productCount');
let productCount = Number(countElement.textContent);
let balance = Number(document.querySelector('#UserBalance').textContent.
replace('Balance: ', '').replace('⚡', ''));
let price = Number(element.querySelector('.productPrice').textContent);*/

function getCount() {
    count += 21;
    return count;
}

async function addProductItemWithIndex(name, information, rating, count, realId, price) {
    await addProductItem(productsId.length, name, information, rating, count, realId, price);
}

async function getProductsFromDB() {
    let result = await (await fetch('/getProductsFromDB')).text();
    productsOnDB = JSON.parse(result);
    for (let i = 0; i < productsOnDB.length; i++) {
        await addProductItemWithIndex(productsOnDB[i].Name, productsOnDB[i].Information, productsOnDB[i].Rating, productsOnDB[i].Count, productsOnDB[i].Id, productsOnDB[i].Price);
    }
    setTimeout(() => {
        balanceElement = document.querySelector('#UserBalance');
        if (balanceElement != null){
            balance = Number(balanceElement.textContent.
            replace('Balance: ', '').replace('⚡', ''));
        }
    }, 500)
    await showRatingAfterLoad();
}

function Product(id, name, information, rating, count, realId, price){
    this.Id = id;
    this.Name = name;
    this.Information = information;
    this.Rating = rating;
    this.Count = count;
    this.RealId = realId;
    this.Price = price;
}

async function makeReview(count, id, productId) {
    let response = fetch("/addProducts",
        { method: "POST", body: JSON.stringify(sendingUserProducts)});
}


let timeout = 1000;
async function changeProductCount(count, id, productId) {
    if (sendingUserProducts.length > timeout / 100)
    {
        sendingUserProducts = [];
        await reloadPage();
    }
    let element = productsOnPage[id];
    let productCount = element.Count;
    let price = element.Price

    let countElement = document.getElementById(id + "product").querySelector('.productCount');
    
/*    if (count < 0 && productCount <= 0){
        let response = await fetch("/deleteUserProduct",
            {method: "POST", body: JSON.stringify(new UserProduct(-1, productId, count))});
        document.querySelector('#UserBalance').textContent = "Balance: " + JSON.parse(await response.text()).Balance + "⚡";
        return;
    }*/
    //if ProductCount On Page > 0
    if (productCount > 0){
        console.log(productCount + " Count");
/*        let response = await fetch("/addProductCount",
            { method: "POST", body: JSON.stringify(new UserProduct(-1, productId, count))});
        if (response.ok){*/
            //let dbData = JSON.parse(await response.text());
            //if we wonna add product CHECK balance
            if (count > 0 && balance - count * price >= 0)
            {
                
/*                document.querySelector('#UserBalance').textContent = "Balance: " + dbData.Balance + "⚡";
                countElement.title = dbData.ProductCount;
                countElement.textContent = dbData.ProductCount;*/
                balance = balance - price * count;
                balanceElement.textContent = "Balance: " + balance + "⚡";
                countElement.title = Number(countElement.title) + count;
                countElement.textContent = Number(countElement.textContent) + count;
                await AddSendingData(productId, count, id);
            }
            else if (count < 0)
            {
/*                document.querySelector('#UserBalance').textContent = "Balance: " + dbData.Balance + "⚡";
                countElement.title = dbData.ProductCount;
                countElement.textContent = dbData.ProductCount;*/
                balance = balance - count * price;
                balanceElement.textContent = "Balance: " + balance + "⚡";
                element.Count = Number(countElement.textContent) + count
                countElement.title = element.Count;
                countElement.textContent = element.Count;
                await AddSendingData(productId, count, id);
                
/*                countElement.title = Number(countElement.title) + count;
                countElement.textContent = Number(countElement.textContent) + count;*/
            }
        }
        else {
            console.log("count < 0")
        if (count > 0 && balance - count * price >= 0)
        {
            /*                document.querySelector('#UserBalance').textContent = "Balance: " + dbData.Balance + "⚡";
                            countElement.title = dbData.ProductCount;
                            countElement.textContent = dbData.ProductCount;*/
            balance = balance - count * price;
            balanceElement.textContent = "Balance: " + balance + "⚡";
            element.Count = Number(countElement.textContent) + count
            countElement.title = element.Count;
            countElement.textContent = element.Count;
            console.log(balance + " after " + count * price);
            await AddSendingData(productId, count, id);
            
/*            countElement.title = Number(countElement.title) + count;
            countElement.textContent = Number(countElement.textContent) + count;*/
        }
    }
/*        else {
            let errorBlock = document.getElementById('errorBlock');
            errorBlock.innerText = "Error!";
            setTimeout(TurnOffErrorText, 3000);
        }
    }*/
}

let sendingUserProducts = [];
async function AddSendingData(productId, productCount, id) { 
    productsOnDB[id].Count += productCount;
    sendingUserProducts.push(new UserProduct(-1, productId, productCount));
    console.log(isSending);
    if (!isSending){
        SendDataToDB();
    }
}
let c = 0;
function SendDataToDB() {
    isSending = true;
    window.setTimeout(() => {
        let response = fetch("/addProducts",
            { method: "POST", body: JSON.stringify(sendingUserProducts)});
        sendingUserProducts = [];
        response.then(response => {if (!response.ok){
            location.reload();
        }}).catch(() =>  {window.clearTimeout(); reloadPage();});
        /*location.reload();
                if (response.ok){
      /*            console.log(response);
                    let result = response.text();
                    console.log(response);
                    let productsOnDB = JSON.parse(result);
                    for (let i = 0; i < productsOnDB.length; i++) {
                        addProductItemWithIndex(productsOnDB[i].Id, productsOnDB[i].Count, productsOnDB[i].Balance);
                    }
        }*/
            c++;
        isSending = false;
    }, 1000)
}

function reloadPage() {
    location.reload();
}

async function TurnOffErrorText() {
    let errorBlock = document.getElementById('errorBlock');
    errorBlock.innerText = "";
}


function UserProduct(userId, productId, productCount){
    this.UserId = userId;
    this.ProductId = productId;
    this.ProductCount = productCount;
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


setInterval(() => {
    try {
        if (!isSending && sendingUserProducts.length === 0) {
            setTimeout(() =>  {
                if (!isSending && sendingUserProducts.length === 0) {
                    fetch('/getProductsFromDB').then(response => response.json()).then(products => {
                        if (!isSending && sendingUserProducts.length === 0) {
                            if (products.length !== productsOnDB.length) {
                                location.reload();
                            }
                            let productsString = JSON.stringify(products);
                            for (let i = 0; i < products.length; i++) {
                                if (!productsString.includes(JSON.stringify(productsOnDB[i]))) {
                                    reloadPage();
                                }
                            }
                        }
                    })
                }
            }, 2 * 1000);
        }
    }
    catch (exception) {
        reloadPage();
    }
}, 60 * 1000);



//window.addEventListener('beforeunload', (evt) => alert(1));
/*
window.onload = alert(1);
window.onbeforeunload = alert(1);
window.onunload = alert(1);
window.onclose = alert(1);*/
