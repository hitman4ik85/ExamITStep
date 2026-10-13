import axios from 'axios';
import * as Device from 'expo-device';
import * as ImagePicker from 'expo-image-picker';
import React, { useEffect, useState } from 'react';
import { Alert, Image, Pressable, StyleSheet, Text, TextInput, View, useWindowDimensions } from 'react-native';
import { SelectList } from 'react-native-dropdown-select-list';

const addPicture = () => {

    const [file, setFile] = useState<string | null>(null); // Зображення з КАМЕРИ
    const [base64, setBase64] = useState<string | null>(null); // Зображення в base64
    const [error, setError] = useState<string | null>(null); // Помилка
    const [tags, setTags] = useState<string>(''); // Теги (введені + обрані)
    const [listTag, setListTag] = useState<string[]>([]); // Список тегів

    // Визначаємо орієнтацію екрану
    const { width, height } = useWindowDimensions();
    const isLandscape = width > height;
    // Визначаємо стилі залежно від орієнтаціі екрану (портрет/ландшафт)
    let contentStyle: any = styles.column;
    if (isLandscape) {
        contentStyle = styles.row;
    }

    // Скидаємо форму на початкові значення
    const resetForm = () => {
        setFile(null);
        setBase64(null);
        setTags('');
        setError(null);
    }

    // Додаємо тег з SelectList у рядок tags (без дублікатів)
    const addTagFromSelect = (item: string) => {
        const trimmed = item.trim();
        if (!trimmed) return;

        setTags((prev) => {
            if (!prev) {
                return trimmed;
            }
            const parts = prev
                .split(',')
                .map(x => x.trim())
                .filter(x => x.length > 0);
            if (parts.includes(trimmed)) {
                return prev;
            }

            return prev + ', ' + trimmed;
        });
    };

    // КАМЕРА (Android + iOS)
    const pickImage = async () => {
        // Запит до КАМЕРИ пристрою (Android + iOS)
        const { status } = await ImagePicker.requestCameraPermissionsAsync();

        if (status !== 'granted') {
            Alert.alert('Permission Denied', 'You did not recived camera permission.');
        }
        else {
            // Відкриваємо саме КАМЕРУ (Android + iOS)
            const result = await ImagePicker.launchCameraAsync({
                allowsEditing: true, // Надає змогу редагувати зображення
                aspect: [4, 3], // Розмір зображення
                quality: 0.5 // Надає змогу збільшити або зменшити якість зображення при завантаженні
            });

            if (!result.canceled) {
                setFile(result.assets[0].uri);
                setError(null);
            }
        }
    }

    // ГАЛЕРЕЯ (Android + iOS)
    const pickImageFromGallery = async () => {
        // Запит до ГАЛЕРЕЇ (Android + iOS)
        const { status } = await ImagePicker.requestMediaLibraryPermissionsAsync();

        if (status !== 'granted') {
            Alert.alert('Permission Denied', 'You did not recived camera permission.');
        }
        else {
            // Відкриваємо саме ГАЛЕРЕЮ (Android + iOS)
            const result = await ImagePicker.launchImageLibraryAsync({
                allowsEditing: true, // Надає змогу редагувати зображення
                aspect: [4, 3], // Розмір зображення
                quality: 0.5 // Надає змогу збільшити або зменшити якість зображення при завантаженні
            });

            if (!result.canceled) {
                setFile(result.assets[0].uri);
                setError(null);
            }
        }
    }

    // Основний метод: перетворення зображення в base64 через axios ->
    const imageToBase64 = async () => {
        try {
            if (!file) return; // Якщо немає зображення, то вихід
            // Інакше перетворюємо зображення в base64
            const response = await axios.get(file, { responseType: 'arraybuffer' });
            const buffer = response.data as ArrayBuffer;
            const bytes = new Uint8Array(buffer);
            const b64 = btoa(
                bytes.reduce((acc, byte) => acc + String.fromCharCode(byte), '')
            );
            setBase64(b64);
        } catch (error) {
            console.log(error);
            setError(error as string);
            return null;
        }
    };

    /*
    // 1) Альтернативний наш(старий) варіант через fetch ->
    const imageToBase64WithFetch = async () => {
        try {
            if (!file) return;
            fetch(file).
                then(response => response.arrayBuffer()).
                then(data => {
                    const b64 = btoa(new Uint8Array(data).reduce((item, byte) => item + String.fromCharCode(byte), ''));
                    setBase64(b64);
                });
        } catch (error) {
            console.log(error);
            setError(error as string);
            return null;
        }
    };
    */

    // Завантаження зображення на сервер
    const uploadImage = async () => {
        if (!base64) return; // Якщо немає base64, то вихід
        if (!tags || tags.trim().length === 0) return; // Якщо немає тегів, то вихід
        try {
            const url = 'http://192.168.0.225:5104/api/pictures';
            const toSend = {
                tags: `${tags}`,
                data: `${base64}`,
                sentby: `${Device.modelName ?? 'Unknown'}`,
                createdat: new Date()
            }
            const response = await axios.post(url, toSend, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            console.log('Success:', response.data);
            resetForm(); // Скидаємо форму після успішного завантаження
            Alert.alert('Success', 'Image uploaded.');
        } catch (error) {
            console.log(error);
            setError(error as string);
            return null;
        }
    };

    // Отримання списку тегів
    const getTagList = async () => {
        const url = 'http://192.168.0.225:5104/api/pictures/tags';
        const response = await axios.get(url, { method: 'GET' });
        if (response.status == 200) {
            const names: string[] = response.data.map((item: { tagName: string }) => item.tagName as string);
            const uniqueNames: string[] = Array.from(new Set<string>(names));
            setListTag(uniqueNames);
        }
        else {
            console.log(response.statusText);
            setError(response.statusText as string);
            return null;
        }
    };

    useEffect(() => { uploadImage() }, [base64]); // Завантаження зображення на сервер
    useEffect(() => { getTagList() }, []); // Отримання списку тегів

    return (
        <View style={styles.container}>
            <Text style={styles.header}>
                Add Image:
            </Text>

            {/* Контейнер для кнопок: вертикально в портреті, горизонтально в ландшафті */}
            <View style={isLandscape ? styles.buttonsRow : styles.buttonsColumn}>
                {/* Кнопка для КАМЕРИ */}
                <Pressable style={styles.button} onPress={pickImage}>
                    <Text style={styles.buttonText}>Open Camera</Text>
                </Pressable>

                {/* Кнопка для ГАЛЕРЕЇ */}
                <Pressable style={styles.button} onPress={pickImageFromGallery}>
                    <Text style={styles.buttonText}>Choose From Gallery</Text>
                </Pressable>
            </View>

            <View style={contentStyle}>
                <View style={isLandscape ? styles.formLandscape : styles.form}>
                    <TextInput
                        onChangeText={(text) => { setTags(text) }}
                        value={tags}
                        placeholder="Enter tags"
                        placeholderTextColor="#e2e8f0"
                        style={styles.input}
                    />
                    <SelectList
                        data={listTag}
                        search={false}
                        save="value"
                        setSelected={(item: string) => { addTagFromSelect(item) }}
                        boxStyles={styles.boxStyles}
                        dropdownStyles={styles.dropdownStyles}
                        inputStyles={{ color: '#e2e8f0' }}
                        dropdownTextStyles={{ color: '#e2e8f0' }}
                    />
                    <Pressable style={styles.button} onPress={() => { imageToBase64() }}>
                        <Text style={styles.buttonText}>Upload Image</Text>
                    </Pressable>
                </View>

                {file ? (
                    <View style={isLandscape ? styles.imageContainerLandscape : styles.imageContainer}>
                        <Image
                            source={{ uri: file }}
                            resizeMode='contain'
                            style={styles.image}
                        />
                    </View>
                ) : (
                    <Text style={styles.errorText}>{error}</Text>
                )}
            </View>
        </View>
    );
}

export default addPicture;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'flex-start',
        alignItems: 'center',
        paddingHorizontal: 16,
        paddingTop: 10,
        paddingBottom: 10, // Запас під таббар
        backgroundColor: '#050016'
    },
    header: {
        color: '#c4b5fd',
        fontSize: 22,
        fontWeight: '700',
        marginTop: 4,
        marginBottom: 6,
        textAlign: 'center',
        textShadowColor: '#a855f7',
        textShadowOffset: { width: 0, height: 0 },
        textShadowRadius: 12
    },

    row: {
        width: '100%',
        flexDirection: 'row',
        alignItems: 'flex-start',
        justifyContent: 'space-between',
        marginTop: 10
    },
    column: {
        width: '100%',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'flex-start',
        marginTop: 10
    },

    // Нові стилі для розташування кнопок
    buttonsRow: {
        width: '100%',
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        marginTop: 4
    },
    buttonsColumn: {
        width: '100%',
        alignItems: 'center'
    },

    button: {
        backgroundColor: '#a855f7',
        paddingVertical: 10,
        paddingHorizontal: 12,
        borderRadius: 16,
        marginBottom: 10,
        marginTop: 8,
        marginHorizontal: 6,
        borderWidth: 1,
        borderColor: '#22d3ee',
        shadowColor: '#22d3ee',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.95,
        shadowRadius: 12,
        elevation: 10
    },
    buttonText: {
        color: '#020617',
        fontSize: 16,
        fontWeight: '700',
        textAlign: 'center'
    },
    imageContainer: {
        width: '95%',
        borderRadius: 18,
        marginTop: 12,
        marginBottom: 8,
        backgroundColor: '#050018',
        padding: 10,
        alignItems: 'center',
        shadowColor: '#22d3ee',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.7,
        shadowRadius: 10,
        elevation: 8,
        borderWidth: 1,
        borderColor: '#a855f7'
    },
    imageContainerLandscape: {
        width: '40%',
        borderRadius: 18,
        marginTop: 0,
        marginBottom: 0,
        backgroundColor: '#050018',
        padding: 10,
        alignItems: 'center',
        shadowColor: '#22d3ee',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.7,
        shadowRadius: 10,
        elevation: 8,
        borderWidth: 1,
        borderColor: '#a855f7'
    },
    image: {
        width: 220,
        height: 220,
        borderRadius: 16
    },
    errorText: {
        color: '#fb7185',
        marginTop: 16
    },
    form: {
        width: '95%',
        backgroundColor: '#050018',
        borderRadius: 16,
        borderWidth: 1,
        borderColor: '#22d3ee',
        padding: 10,
        shadowColor: '#4f46e5',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.5,
        shadowRadius: 12,
        elevation: 6
    },
    formLandscape: {
        width: '55%',
        backgroundColor: '#050018',
        borderRadius: 16,
        borderWidth: 1,
        borderColor: '#22d3ee',
        padding: 10,
        shadowColor: '#4f46e5',
        shadowOffset: { width: 0, height: 0 },
        shadowOpacity: 0.5,
        shadowRadius: 12,
        elevation: 6
    },
    input: {
        borderRadius: 10,
        borderWidth: 1,
        borderColor: '#2dd4bf',
        backgroundColor: '#020617',
        paddingVertical: 6,
        paddingHorizontal: 8,
        marginBottom: 8,
        color: '#e2e8f0'
    },
    boxStyles: {
        borderRadius: 10,
        borderColor: '#a855f7',
        backgroundColor: '#020617'
    },
    dropdownStyles: {
        borderRadius: 10,
        borderColor: '#a855f7',
        backgroundColor: '#020617'
    }
});