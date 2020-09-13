import Filter from "./filter.js";

const InfoPage = function (helps) {
    this.items = helps;
};

InfoPage.prototype.renderItems = function () {
    let fragment = document.createDocumentFragment();
    let list = document.getElementById("info__list");
    list.textContent = '';
    for (let item of this.items) {
        fragment.appendChild(item.render());
    }
    list.appendChild(fragment);
};


export default InfoPage;