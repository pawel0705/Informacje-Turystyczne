import Util from "./utility.js";

const User = function (name) {
    this.name = name;
    this.Id = 0;
    this.regionPermissions = {};
    this.permissionsExpanded = false;
    this.addUrl = "";
    this.removeUrl = "";
}

const postPermission = function (url, user, regionName) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
        id: user,
        regionName: regionName
    }));
    console.log(`posting ${user} ${regionName} to ${url}`);
    /*var stringArray = new Array();
    stringArray[0] = "item1";
    stringArray[1] = "item2";
    stringArray[2] = "item3";
    var postData = { values: stringArray };

    $.ajax({
        type: "POST",
        url: "/Home/SaveList",
        data: postData,
        dataType: "json",
        traditional: true
    });*/
}

User.prototype.renderCheckbox = function (content, checked, id, name) {
    let checkbox = Util.createElement("input");
    checkbox.type = "checkbox";
    checkbox.id = id;
    checkbox.name = name;
    checkbox.value = name;
    checkbox.checked = checked;
    checkbox.addEventListener("change", () => {
        this.regionPermissions[checkbox.name] = !this.regionPermissions[checkbox.name];
        if (this.regionPermissions[checkbox.name]) {
            postPermission(this.addUrl, this.id, checkbox.name);
        } else {
            postPermission(this.removeUrl, this.id, checkbox.name);
        }
        //this.makeDirty();
    });
    content.appendChild(checkbox);
    let label = Util.createElement("label", { withText: name });
    label.htmlFor = id;
    content.appendChild(label);
    content.appendChild(Util.createElement("br"));
}

User.prototype.render = function () {
    let itemDiv = Util.createElement("div", { withClass: "info__item" });

    //itemDiv.appendChild(Util.createElement("h1", { withText: this.name }));
    //let deleteButton = Util.createElement("button", { withText: "Usuń", withClass: ["info__delete"] });
    //itemDiv.appendChild(deleteButton);
    let button = Util.createElement("button", { withText: this.name, withClass: ["toggle-button", "expand__button"] });

    itemDiv.appendChild(button);

    let content = Util.createElement("div", { withClass: "expand__content" });
    for (let [key, val] of Object.entries(this.regionPermissions)) {
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

export default User;