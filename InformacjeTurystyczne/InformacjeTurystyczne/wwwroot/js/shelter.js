import Util from "./utility.js";

const Shelter = function () {
    this.type = "schronisko";
    this.name = "N/A";
    this.region = "N/A";
    this.places = "N/A";
    this.maxPlaces = "N/A";
    this.open = "N/A";
    this.phoneNumber = "N/A";
    this.description = "N/A";
}

Shelter.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    itemDiv.appendChild(Util.createElement("p", { withText: this.type, withClass: "info__type" }));
    itemDiv.appendChild(Util.createRow("region:", this.region));
    itemDiv.appendChild(Util.createRow("wolne miejsca:", this.places));
    itemDiv.appendChild(Util.createRow("wszystkich miejsc:", this.maxPlaces));
    itemDiv.appendChild(Util.createRow("otwarte:", this.open));
    itemDiv.appendChild(Util.createRow("telefon:", this.phoneNumber));
    itemDiv.appendChild(Util.createRow("opis:", this.description));

    return itemDiv;
}

export default Shelter;