
class GlobalScopes {
    constructor() {
        this.eventListenerState = {
            menuLinks: { state: false },
            logout: { state: false },
            rideSendButton: { state: false },
            logoutButton: { state: false },
            cardLeftButton: { state: false },
            cardRightButton: { state: false },
            cardLeftButtonMobileVersion: { state: false },
            cardRightButtonMobileVersion: { state: false },
            cardChangeModeStudyButton: { state: false },
            cardChangeModeReviewButton: { state: false },
            studyButtonUnknow: { state: false },
            studyButtonKnow: { state: false },
            studyButtonNextItem: { state: false },
            itemListFirstButton: { state: false },
            itemListPrevButton: { state: false },
            itemListNextButton: { state: false },
            itemListLastButton: { state: false },
            itemListButtonFromListToCard: { state: false },
            itemListPutButton: { state: false },
            itemListDeleteButton: { state: false },
            itemListAddButton: { state: false },
            formPutButton: { state: false },
            formAddButton: { state: false }
        }// used in addBubleEventListener() for add once event listener caller

        this.wordOrder = {
            length: 0,
            isFromCarDetails: false
        }; // used in pageHandlers['word-list'] for eventListener
    }; 

    getEventListenerState() {
        return this.eventListenerState;
    }

    getWordOrder() {
        return this.wordOrder;
    }

}

var globalScopes = new GlobalScopes();

export default globalScopes;