export const HtmlUtil = {
    addClassToBody: (className: string) => {
        document.body.classList.add(className);
    },

    removeClassFromBody: function (className: string) {
        document.body.classList.remove(className);
    },

    hasClass: function (className: string): boolean {
        return document.body.classList.contains(className);
    }
}