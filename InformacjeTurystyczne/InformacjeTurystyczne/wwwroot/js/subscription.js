import Util from "./utility.js";

const Subscription = function (name) {
    //this.name = name;
    //this.Id = 0;
    this.regionSubscriptions = {};
    this.regions = {};
    this.permissionsExpanded = false;
    this.addUrl = "";
    this.removeUrl = "";
    this.onApply = () => { };
}

const postSubscription = function (url, regionName) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
        regionName: regionName
    }));
    console.log(`posting ${regionName} to ${url}`);
}

Subscription.prototype.renderCheckbox = function (content, checked, id, name) {
    let checkbox = Util.createElement("input");
    checkbox.type = "checkbox";
    checkbox.id = id;
    checkbox.name = name;
    checkbox.value = name;
    checkbox.checked = checked;
    checkbox.addEventListener("change", () => {
        this.regionSubscriptions[checkbox.name] = !this.regionSubscriptions[checkbox.name];
        if (this.regionSubscriptions[checkbox.name]) {
            postSubscription(this.addUrl, checkbox.name);
        } else {
            postSubscription(this.removeUrl, checkbox.name);
        }
        //this.makeDirty();
    });
    content.appendChild(checkbox);
    let label = Util.createElement("label", { withText: name });
    label.htmlFor = id;
    content.appendChild(label);
    content.appendChild(Util.createElement("br"));
};

Subscription.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "expand" });

    let button = Util.createElement("button", { withText: "Edytuj Subskrypcje", withClass: ["toggle-button", "expand__button"] });
    let applyButton = Util.createElement("button", { withText: "Pokaż Subskrypowane", withClass: ["expand"] });
    itemDiv.appendChild(applyButton);

    applyButton.addEventListener("click", () => {
        this.onApply();
        //this.makeDirty();
    });

    itemDiv.appendChild(button);

    let content = Util.createElement("div", { withClass: "expand__content" });
    for (let [key, val] of Object.entries(this.regionSubscriptions)) {
        this.renderCheckbox(content, val, `${this.name}_${key}`, key);
    }
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

export default Subscription;