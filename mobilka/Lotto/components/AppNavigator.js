import React from 'react';
import { createBottomTabNavigator, createAppContainer } from 'react-navigation';
import {Image} from 'react-native'
import HomeScreen from '../screens/HomeScreen';
import colors from '../helper/colors';
import StatisticsScreen from '../screens/StatisticsScreen';
import WalletScreen from '../screens/WalletScreen';
import HelpScreen from '../screens/HelpScreen';

const TabNavigator = createBottomTabNavigator({
    Home: HomeScreen,
    Statistics: StatisticsScreen,
    Wallet: WalletScreen,
    Help: HelpScreen
  },{
    defaultNavigationOptions: ({ navigation }) => ({
      tabBarIcon: ({ focused, horizontal, tintColor }) => {
        const { routeName } = navigation.state;
        if (routeName === 'Home') {
            return <Image source={focused?require("../assets/tab1active.png"):require("../assets/tab1.png")} resizeMode={"contain"} style={{height:20, width:20}} />
        } else if (routeName === 'Statistics') {
          return <Image source={focused?require("../assets/tab2active.png"):require("../assets/tab2.png")} resizeMode={"contain"} style={{height:20, width:20}} />
        } else if (routeName === 'Wallet') {
          return <Image source={require("../assets/tab3.png")} resizeMode={"contain"} style={{height:20, width:20}} />
        } else if (routeName === 'Help') {
          return <Image source={require("../assets/tab4.png")} resizeMode={"contain"} style={{height:20, width:20}} />
        } 
      },
    }),
    tabBarOptions: {
      showLabel: false,
      activeTintColor: colors.colorAccent,
      inactiveTintColor: 'gray',
    },
  });
  
  export default createAppContainer(TabNavigator);