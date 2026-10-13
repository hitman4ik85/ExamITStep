import { useLocalSearchParams } from 'expo-router'
import React, { useEffect, useState } from 'react'
import { Image, StyleSheet, Text, View, useWindowDimensions } from 'react-native'
import axios from 'axios'
// Інтерфейс деталей зображення
interface PictureDetails {
    id: number
    tags: string
    data: string
    sentBy: string
    createdAt: string
}

// Отримання деталей зображення
const infoPicture = () => {

    const { Id } = useLocalSearchParams(); // Отримання параметрів URL зображення по ID
    const [item, setItem] = useState<PictureDetails | null>(null); // Дані зображення з сервера
    const [error, setError] = useState<Error | null>(null); // Помилка

    // Визначаємо орієнтацію екрану
    const { width, height } = useWindowDimensions()
    const isLandscape = width > height

    // Отримання деталей зображення
    const loadDetails = async () => {
        try {
            setError(null)
            const url = `http://192.168.0.225:5104/api/pictures/${Id}`

            // Отримання деталей зображення з сервера (метод GET)
            const response = await axios.get(url, { method: 'GET' })
            if (response.status !== 200) throw new Error(response.statusText)
            setItem(response.data)
        } catch (error) {
            console.log(error)
            setError(error as Error)
            setItem(null)
        }
    }

    useEffect(() => { loadDetails() }, [Id]) // Виклик методу при завантаженні компонента

    return (
        <View style={styles.container}>
            {item ? (
                <>
                    <Image
                        source={{ uri: `data:image/jpeg;base64,${item.data}` }}
                        resizeMode='contain'
                        style={styles.image}
                    />
                    <View style={isLandscape ? styles.infoRow : styles.infoColumn}>
                        <Text style={[styles.text, isLandscape && styles.rowText]}>
                            <Text style={styles.label}>Tags: </Text>
                            {item.tags}
                        </Text>
                        <Text style={[styles.text, isLandscape && styles.rowText]}>
                            <Text style={styles.label}>by: </Text>
                            {item.sentBy}
                        </Text>
                        <Text style={[styles.text, isLandscape && styles.rowText]}>
                            <Text style={styles.label}>Create: </Text>
                            {new Date(item.createdAt).toLocaleString()}
                        </Text>
                    </View>
                </>
            ) : (
                <Text style={styles.text}>
                    {error ? error.message : 'No data'}
                </Text>
            )}
        </View>
    )
}

export default infoPicture;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#050016',
        alignItems: 'center',
        justifyContent: 'flex-start',
        paddingTop: 10,
        paddingHorizontal: 10,
        paddingBottom: 10 // Запас під таббар
    },
    image: {
        width: '92%',
        height: '70%',
        marginTop: 4,
        marginBottom: 8,
        borderRadius: 20,
        borderWidth: 1.5,
        borderColor: '#a855f7'
    },

    // Колонки (портрет)
    infoColumn: {
        width: '92%',
        alignItems: 'center'
    },

    // Рядок (ландшафт)
    infoRow: {
        width: '92%',
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        marginTop: 5
    },

    // Додаткові правки тексту для рядка
    rowText: {
        width: 'auto',
        marginTop: 0,
        marginBottom: 0,
        paddingHorizontal: 6,
        textAlign: 'left'
    },

    text: {
        marginTop: 10,
        marginBottom: 5,
        width: '92%',
        padding: 5,
        fontWeight: 600 as any,
        color: '#e2e8f0',
        fontSize: 16,
        textAlign: 'center',
        textShadowColor: '#22d3ee',
        textShadowOffset: { width: 0, height: 0 },
        textShadowRadius: 8
    },
    label: {
        color: '#22d3ee',
        fontWeight: '700'
    }
});