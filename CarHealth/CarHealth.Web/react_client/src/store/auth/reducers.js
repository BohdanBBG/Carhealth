
import {
    ACTION_SET_APP_CONFIG,
    ACTION_SET_ACCESS_TOKEN
} from '../auth/actions.js';

const initialState = { // if we skip it, the application will return undefined for states on first launch
    appConfig: {},
    accessToken: ""
};


export const authReducer = (state = initialState, action) => {

    switch (action.type) {
        case ACTION_SET_APP_CONFIG:
            return {
                ...state,
                appConfig: action.payload
            };
        case ACTION_SET_ACCESS_TOKEN:
            return {
                ...state,
                accessToken: action.payload
             };
    }

    return state;
};