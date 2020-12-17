
import {
    ACTION_CHANGE_FIRST_NAME,
    ACTION_CHANGE_SECOND_NAME
} from '../examples/actions.js';

const initialState = { // if we skip it, the application will return undefined for states on first launch
    firstName: 'NameEx',
    secondName: 'SecondEx'
};


export const exampleReducer = (state = initialState, action) => {

    switch (action.type) {
        case ACTION_CHANGE_FIRST_NAME:
            return {
                ...state,
                firstName: action.payload
            };
        case ACTION_CHANGE_SECOND_NAME:
            return {
                ...state,
                secondName: action.payload
             };
    }

    return state;
};