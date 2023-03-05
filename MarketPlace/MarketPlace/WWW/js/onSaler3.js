let canChange = true;
let counter = 0.01;

async function moveOpacity() {
    if (canChange){
        canChange = false;
        let opacity = Number(document.querySelector("#backSalesPeople3").style.opacity);
        addCount(opacity);
    }
}

function addCount(opacity) {
    if (opacity < 1)
    {
        setTimeout(() => {
            counter += 0.01
            opacity += counter
            document.querySelector("#backSalesPeople3").style.opacity = opacity;
            addCount(opacity, counter);
        }, 50);
    } else {
        counter = 0.01;
        deleteCount(opacity);
    }
}

function deleteCount(opacity) {
    if (opacity > 0){
        setTimeout(() => {
            counter += 0.01
            opacity -= counter
            document.querySelector("#backSalesPeople3").style.opacity = opacity;
            deleteCount(opacity);
        }, 50);
    }
    else {
        canChange = true;
    }
}


let canChangeHead = true;
let headCounter = 0.01;
async function ChangeHalmetState() {
    if (canChangeHead){
        canChangeHead = false;
        let opacity = Number(document.querySelector("#backSalesPeople3Head").style.opacity);
        addCountHead(opacity);
    }
}

function addCountHeadCheck() {
    if (canChangeHead){
        let opacity = Number(document.querySelector("#backSalesPeople3Head").style.opacity);
        canChangeHead = false;
        addCountHead(opacity);
    }
}

function deleteCountHeadCheck() {
    if (canChangeHead){
        let opacity = Number(document.querySelector("#backSalesPeople3Head").style.opacity);
        canChangeHead = false;
        deleteCountHead(opacity);
    }
}
function addCountHead(opacity) {
    console.log(opacity + " c " + canChangeHead);
    if (opacity < 1)
    {
        console.log(2);
        setTimeout(() => {
            headCounter += 0.05
            opacity += headCounter
            document.querySelector("#backSalesPeople3Head").style.opacity = opacity;
            addCountHead(opacity, headCounter);
        }, 100);
    }
    else {
        canChangeHead = true;
        headCounter = 0.01;
    }
}

function deleteCountHead(opacity) {
    if (opacity > 0){
        setTimeout(() => {
            headCounter += 0.05
            opacity -= headCounter
            document.querySelector("#backSalesPeople3Head").style.opacity = opacity;
            deleteCountHead(opacity);
        }, 100);
    }
    else {
        canChangeHead = true;
        headCounter = 0.01;
    }
}