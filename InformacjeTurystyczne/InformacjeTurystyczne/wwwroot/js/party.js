import Util from "./utility.js";

const Party = function () {
    this.type = "impreza";
    this.name = "N/A";
    this.region = "N/A";
    this.upToDate = "N/A";
    this.placeDescription = "N/A";
    this.description = "N/A";
}

Party.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    itemDiv.appendChild(Util.createElement("p", { withText: this.type, withClass: "info__type" }));
    itemDiv.appendChild(Util.createRow("region:", this.region));
    itemDiv.appendChild(Util.createRow("aktualna:", this.upToDate));
    itemDiv.appendChild(Util.createRow("opis:", this.description));
    itemDiv.appendChild(Util.createRow("miejsce:", this.placeDescription));

    return itemDiv;
}

export default Party;