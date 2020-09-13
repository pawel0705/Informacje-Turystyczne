
const Utility = {};

Utility.createElement = function (tag, options) {

    options = options || {};
    const { withText, withClass } = options;

    let element = document.createElement(tag);
    if (withText) {
        let text = document.createTextNode(withText);
        element.appendChild(text);
    }
    if (withClass) {
        if (Array.isArray(withClass)) {
            element.classList.add(...withClass);
        } else {
            element.classList.add(withClass);
        }
    }
    return element;
}

Utility.createRow = function (...texts) {
    let rowDiv = Utility.createElement("div", { withClass: "row" });
    for (let text of texts) {
        rowDiv.appendChild(Utility.createElement("span", { withText: text }));
    }
    return rowDiv;
}

Utility.remove = function (array, ...items) {
    for (let i = items.length - 1; i >= 0; i--) {
        let what = items[i];
        let whatIndex = array.indexOf(what);
        while (whatIndex !== -1) {
            array.splice(whatIndex, 1);
            whatIndex = array.indexOf(what);
        }
    }
}

export default Utility;