import React from 'react';
import {Image} from 'react-native'
import { Marker, Circle } from 'react-native-maps';
import colors from '../helper/colors';

export default ({ ...props }) => {
return (
    <Circle center={props.coordinate} radius={6000} strokeWidth={0} fillColor={colors.colorAccent}/>
)}