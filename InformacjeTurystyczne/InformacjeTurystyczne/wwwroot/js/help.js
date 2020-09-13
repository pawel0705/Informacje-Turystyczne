import Util from "./utility.js";

const Help = function (name) {
    this.name = name;
    this.content = "";
}

Help.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    let button = Util.createElement("button", { withText: this.name, withClass: ["toggle-button", "expand__button"] });

    itemDiv.appendChild(button);

    let content = Util.createElement("div", { withClass: "expand__content" });
    let text = Util.createElement("h1", { withText: this.content });
    content.appendChild(text);
    itemDiv.appendChild(content);

    if (this.permissionsExpanded) {
        button.classList.add("toggle-button--active");
        content.classList.add("show");
    }

    button.addEventListener("click", (e) => {
        this.permissionsExpanded = !this.permissionsExpanded;
        button.classList.toggle("toggle-button--active");
        content.classList.toggle("show");
    });

    return itemDiv;
}

export default Help;