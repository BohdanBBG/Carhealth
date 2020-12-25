
import {
    ACTION_SET_APP_CONFIG,
} from '../auth/actions.js';

const initialState = { // if we skip it, the application will return undefined for states on first launch
    appConfig: {},
};


export const authReducer = (state = initialState, action) => {

    switch (action.type) {
        case ACTION_SET_APP_CONFIG:
            return {
                ...state,
                appConfig: action.payload
            };
      
        default: {
            return state;
        }
    }

};