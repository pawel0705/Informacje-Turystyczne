import Util from "./utility.js";

const Filter = function (name, propertyName, ...items) {
    this.name = name;
    this.propertyName = propertyName;
    this.items = {};
    for (let item of items) {
        this.items[item] = true;
    }
    this.activeItems = [...items];
    this.inactiveItems = [];
    this.selected = false;
    this.onDirty = () => { };
    this.expanded = false;
}

Filter.prototype.makeDirty = function () {
    this.onDirty();
}

Filter.prototype.renderFilterItem = function (content, checked, item) {
    let checkbox = Util.createElement("input");
    checkbox.type = "checkbox";
    checkbox.id = item;
    checkbox.name = item;
    checkbox.value = item;
    checkbox.checked = checked;
    checkbox.addEventListener("change", () => {
        this.items[checkbox.id] = !this.items[checkbox.id];
        this.makeDirty();
    });
    content.appendChild(checkbox);
    let label = Util.createElement("label", { withText: item });
    label.htmlFor = item;
    content.appendChild(label);
    content.appendChild(Util.createElement("br"));
}

Filter.prototype.render = function () {
    let filterDiv = Util.createElement("div", { withClass: ["expand", "filteritem"] });

    let button = Util.createElement("button", { withText: this.name, withClass: ["toggle-button", "expand__button"] });
    
    filterDiv.appendChild(button);

    let content = Util.createElement("div", { withClass: "expand__content" });
    for (let [key, val] of Object.entries(this.items)) {
        this.renderFilterItem(content, val, key);
    }
    //this.renderFilterItems(content, true, this.activeItems);
    //this.renderFilterItems(content, false, this.inactiveItems);
    filterDiv.appendChild(content);

    if (this.expanded) {
        button.classList.add("toggle-button--active");
        content.classList.add("show");
    }

    button.addEventListener("click", (e) => {
        this.expanded = !this.expanded;
        button.classList.toggle("toggle-button--active");
        content.classList.toggle("show");
    });

    return filterDiv;
}

Filter.prototype.check = function (item) {
    //return this.activeItems.includes(item);
    if (Array.isArray(item)) {
        for (let i of item) {
            if (this.items[i]) {
                return true;
            }
        }
        return false;
    }
    return this.items[item];
}

export default Filter;