import { Ionicons } from '@expo/vector-icons'
import { Tabs } from 'expo-router'
import * as ScreenOrientation from 'expo-screen-orientation'
import React, { useEffect, useState } from 'react'
import { StyleSheet } from 'react-native'

const RootLayout = () => {

  const [orientation, setOrientation] = useState<ScreenOrientation.Orientation | null>(null) // Змінна для орієнтації екрану

  // Юзефект для зміни орієнтації
  useEffect(() => {
    const handleOrientationChange = (ori: any) => {
      setOrientation(ori.orientationInfo.orientation)
    }
    // Перевірка орієнтації
    const checkOrientation = async () => {
      const currentOrientation = await ScreenOrientation.getOrientationAsync()
      console.log('Current orientation: ', currentOrientation)
      setOrientation(currentOrientation)
    }
    // Розблокування орієнтації
    const unlockOrientation = async () => {
      await ScreenOrientation.unlockAsync()
      checkOrientation()
    }
    // Підписка на зміну орієнтації
    const subscription = ScreenOrientation.addOrientationChangeListener(handleOrientationChange)
    unlockOrientation()

    return () => {
      // Отримання орієнтації
      ScreenOrientation.removeOrientationChangeListener(subscription)
    }
  }, [])

  // Визначаємо орієнтацію екрану
  const isLandscape =
    orientation === ScreenOrientation.Orientation.LANDSCAPE_LEFT ||
    orientation === ScreenOrientation.Orientation.LANDSCAPE_RIGHT

  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarItemStyle: styles.tabItem,
        tabBarStyle: isLandscape ? styles.tabBarLandscape : styles.tabBar
      }}
    >
      <Tabs.Screen
        name="index"
        options={{
          title: '',
          tabBarIcon: ({ color }) => (<Ionicons name="home" color={color} size={30} />)
        }}
      />
      <Tabs.Screen
        name="addPicture"
        options={{
          title: '',
          tabBarIcon: ({ color }) => (<Ionicons name="add-circle" color={color} size={30} />)
        }}
      />
      <Tabs.Screen
        name="infoPicture"
        options={{
          title: '',
          href: null,
          tabBarIcon: ({ color }) => (<Ionicons name="images" color={color} size={30} />)
        }}
      />
    </Tabs>
  )
}

export default RootLayout;

const styles = StyleSheet.create({
  tabItem: {
    width: '100%',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center'
  },
  tabBar: {
    backgroundColor: '#050016', // Темно-фіолетовий фон
    borderRadius: 40,
    marginHorizontal: 10,
    position: 'absolute',
    bottom: 5,
    height: 55,
    alignSelf: 'center',
    shadowColor: '#a855f7', // Фіолетове світло
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0.7,
    shadowRadius: 14,
    elevation: 10,
    borderWidth: 1.5,
    borderColor: '#22d3ee' // Бірюзовий бордер
  },
  tabBarLandscape: {
    backgroundColor: '#050016',
    borderRadius: 40,
    marginHorizontal: 40,
    position: 'absolute',
    bottom: 8,
    height: 60,
    alignSelf: 'center',
    shadowColor: '#a855f7',
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0.7,
    shadowRadius: 14,
    elevation: 10,
    borderWidth: 1.5,
    borderColor: '#22d3ee'
  }
});