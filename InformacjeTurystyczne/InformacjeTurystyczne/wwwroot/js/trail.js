import Util from "./utility.js";

const Trail = function () {
    this.type = "szlak";
    this.name = "N/A";
    this.colour = "N/A"
    this.length = "N/A";
    this.difficulty = "N/A";
    this.open = "N/A";
    this.feedback = "N/A";
    this.description = "N/A";
    this.region = "N/A";
}

Trail.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    itemDiv.appendChild(Util.createElement("p", { withText: this.type, withClass: "info__type" }));
    if (Array.isArray(this.region)) {
        itemDiv.appendChild(Util.createRow("region:", this.region.join(", ")));
    } else {
        itemDiv.appendChild(Util.createRow("region:", this.region));
    }
    itemDiv.appendChild(Util.createRow("kolor:", this.colour));
    itemDiv.appendChild(Util.createRow("długość:", this.length));
    itemDiv.appendChild(Util.createRow("poziom trudności:", this.difficulty));
    itemDiv.appendChild(Util.createRow("otwarty:", this.open));
    if (this.open != "True") {
        itemDiv.appendChild(Util.createRow("przyczyna:", this.feedback));
    }
    itemDiv.appendChild(Util.createRow("opis:", this.description));

    return itemDiv;
}

export default Trail;