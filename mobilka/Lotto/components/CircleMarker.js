import React from 'react';
import {Image} from 'react-native'
import { Marker, Circle } from 'react-native-maps';
import colors from '../helper/colors';

export default ({ ...props }) => {
    return (
        <Circle center={props.coordinate} radius={36000} strokeWidth={18} fillColor={colors.inCircle} strokeColor={colors.outCircle}/>
    )}