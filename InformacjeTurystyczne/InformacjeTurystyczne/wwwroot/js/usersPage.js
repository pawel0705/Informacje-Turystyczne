import Filter from "./filter.js";

const InfoPage = function (users) {
    this.items = users;
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

InfoPage.prototype.renderFilters = function () {
    /*let sidebar = document.getElementsByClassName("sidebar")[0];
    for (let child of sidebar.querySelectorAll(".expand")) {
        sidebar.removeChild(child);
    }

    sidebar.prepend(this.regionFilter.render());*/
}

export default InfoPage;