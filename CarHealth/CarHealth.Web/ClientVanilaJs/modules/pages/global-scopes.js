
class GlobalScopes {
    constructor() {
        this.eventListenerState = {
            menuLinks: { state: false },
            logout: { state: false },
            AdminsOnly: { state: false },
            logoutButton: { state: false },
           
            itemListFirstButton: { state: false },
            itemListPrevButton: { state: false },
            itemListNextButton: { state: false },
            itemListLastButton: { state: false },
            itemListButtonFromListToCard: { state: false },
            itemListPutButton: { state: false },
            itemListDeleteButton: { state: false },
            itemListAddButton: { state: false },
            formPutButton: { state: false },
            formAddButton: { state: false },
            findCarItemInput: { state: false },

            carManager:{ state: false },
            carManagerMenuItem: { state: false },
            carManagerAddbutton: { state: false },
            carManagerPutbutton: { state: false },
            carManagerDeletebutton: { state: false },
            carManagmendFormPushNewCarButtonEl:  { state: false }

        }// used in addBubleEventListener() for add once event listener caller

    }; 

    getEventListenerState() {
        return this.eventListenerState;
    }

  

}

var globalScopes = new GlobalScopes();

export default globalScopes;