import Util from "./utility.js";

const Attraction = function (type) {
    this.type = type;
    this.name = "N/A";
    this.region = "N/A";
    this.description = "N/A";
}

Attraction.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    itemDiv.appendChild(Util.createElement("p", { withText: this.type, withClass: "info__type" }));
    itemDiv.appendChild(Util.createRow("region:", this.region));
    itemDiv.appendChild(Util.createRow("opis:", this.description));

    return itemDiv;
}

export default Attraction;