

class DomUtil {
    constructor() {

    }

    addBubleEventListener(sourceElSelector, targetElSelector, eventName, checkedHandler, eventHandler) {

        if (!checkedHandler.state) {
            var sourceEl = (typeof sourceElSelector === "object") ? sourceElSelector : document.querySelector(sourceElSelector);
            checkedHandler.state = true;

            sourceEl.addEventListener(eventName, function (e) {
                var actualEl = e.target; // element event fired on
                var desiredEl = e.target.closest(targetElSelector); // element we excpect event fired on

                var matches = actualEl.matches(targetElSelector);
                var isChildEl = desiredEl !== null; // if this el is parent

                if (matches || isChildEl) {
                    eventHandler(e, actualEl, desiredEl || actualEl);
                }
            });
        }
    }
}

export default DomUtil;