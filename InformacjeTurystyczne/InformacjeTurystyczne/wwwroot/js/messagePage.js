import Filter from "./filter.js";
import Subscription from "./subscription.js"

const InfoPage = function (items, regions, subscriptions) {
    this.regions = regions;
    this.items = items;
    this.regionFilter = new Filter("Region", "region", ...regions);
    this.subscriptions = subscriptions;

    this.regionFilter.onDirty = () => {
        this.renderFilters();
        this.renderItems();
    };
    this.subscriptions.onApply = () => {
        this.renderFilters();
        this.renderItemsSubs();
    }
};

InfoPage.prototype.renderItems = function () {
    let fragment = document.createDocumentFragment();
    let list = document.getElementById("info__list");
    list.textContent = '';
    for (let item of this.items.filter(item => this.regionFilter.check(item.region))) {
        fragment.appendChild(item.render());
    }
    list.appendChild(fragment);
};
InfoPage.prototype.renderItemsSubs = function () {
    let fragment = document.createDocumentFragment();
    let list = document.getElementById("info__list");
    list.textContent = '';
    for (let item of this.items.filter(item => this.subscriptions.regionSubscriptions[item.region])) {
        fragment.appendChild(item.render());
    }
    list.appendChild(fragment);
};

InfoPage.prototype.renderFilters = function () {
    let sidebar = document.getElementsByClassName("sidebar")[0];
    for (let child of sidebar.querySelectorAll(".filteritem")) {
        sidebar.removeChild(child);
    }

    sidebar.prepend(this.regionFilter.render());
}
InfoPage.prototype.renderSubs = function () {
    let sidebar = document.getElementsByClassName("sidebar")[0];
    let subs = this.subscriptions.render();
    sidebar.appendChild(subs);
}

export default InfoPage;