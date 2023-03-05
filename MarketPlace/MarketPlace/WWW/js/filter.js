firstIsChecked = true;

async function check(number) {
    console.log(number);
    let firstCheckBox = document.querySelector("#checkBoxRating");
    let secondCheckBox = document.querySelector("#checkBoxPrice");
    if (firstCheckBox.checked && number === 0) {
        secondCheckBox.checked = false;
    }
    if (secondCheckBox.checked && number === 1) {
        firstCheckBox.checked = false;
    }
}

async function resetFilter() {
    let checkBox = document.querySelectorAll(".checkBox");
    let findElement = document.querySelector("#searchElement");
    for (let i = 0; i < checkBox.length; i++) {
        checkBox[i].checked = false;
    }
    findElement.value = null;
}