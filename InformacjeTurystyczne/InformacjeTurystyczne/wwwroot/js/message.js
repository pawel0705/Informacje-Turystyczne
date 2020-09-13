import Util from "./utility.js";

const Message = function () {
    this.name = "N/A";
    this.region = "N/A";
    this.description = "N/A";
    this.category = "N/A";
    this.date = "N/A";
}

Message.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    itemDiv.appendChild(Util.createElement("p", { withText: this.category, withClass: "info__type" }));
    itemDiv.appendChild(Util.createRow("region:", this.region));
    itemDiv.appendChild(Util.createRow("data:", this.date));
    itemDiv.appendChild(Util.createRow("treść:", this.description));

    return itemDiv;
}

export default Message;