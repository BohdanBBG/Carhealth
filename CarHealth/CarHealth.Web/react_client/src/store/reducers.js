
import { combineReducers } from 'redux';
import { exampleReducer } from './examples/reducers.js';
import { authReducer } from './auth/reducers.js';


export default combineReducers({
    example: exampleReducer,
    config: authReducer
});