import { useNavigation } from "@react-navigation/native";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { ActivityIndicator, FlatList, StyleSheet, Text, TextInput, View, useWindowDimensions } from "react-native";
import Card, { Picture } from "../service/card";

const Index = () => {

  const [list, setList] = useState<Picture[] | null>(null); // Список зображень
  const [error, setError] = useState<Error | null>(null); // Помилка
  const [loading, setLoading] = useState(false); // Завантаження зображень
  const [searchTag, setSearchTag] = useState<string>(''); // Пошук по тегу

  const nav = useNavigation(); // Навігація між сторінками

  // Орієнтація екрану
  const { width, height } = useWindowDimensions();
  const isLandscape = width > height;

  // Нормалізація тегу
  const normalizeTag = (value: string) => {
    const tag = value.trim().toLowerCase();
    if (!tag) return '';
    if (tag.startsWith('#')) {
      return tag;
    }
    return '#' + tag;
  };

  // Отримання списку зображень
  const loadPictures = async () => {
    try {
      setLoading(true);
      setError(null);

      const url = 'http://192.168.0.225:5104/api/pictures';

      // Отримання списку зображень з сервера (метод GET)
      const response = await axios.get(url, { method: 'GET' });
      if (response.status !== 200) {
        throw new Error(`Failed to load pictures: ${response.statusText}`);
      }
      const data = response.data;
      // Мапування даних зображень на об'єкти Picture
      const mapped: Picture[] = (data as any[]).map((x) => ({
        Id: x.id ?? x.Id,
        Tags: x.tags ?? x.Tags,
        Data: x.data ?? x.Data,
        SentBy: x.sentBy ?? x.SentBy,
        CreatedAt: x.createdAt ?? x.CreatedAt
      }));

      setList(mapped);
    }
    catch (error) {
      console.log(error);
      setError(error as Error);
      return null;
    }
    finally {
      setLoading(false);
    }
  }
  // Автоматичне завантаження списку зображень
  useEffect(() => { loadPictures(); }, []);

  // Оновлюємо список ПРИ НАТИСКАННІ на кнопку(tabPress) знизу сторінки Дім
  useEffect(() => {
    const unsub = (nav as any).addListener('tabPress', () => {
      loadPictures();
    });
    return unsub;
  }, [nav]);

  // Рендер контенту сторінки зображень
  const renderContent = () => {
    if (loading) {
      return (
        <View style={styles.center}>
          <ActivityIndicator size="large" />
          <Text style={styles.loadingText}>Loading...</Text>
        </View>
      );
    }
    if (error) {
      return (
        <View style={styles.center}>
          <Text style={styles.errorText}>Error: {error.message}</Text>
        </View>
      );
    }
    if (list && list.length > 0) {

      const normalizedSearch = normalizeTag(searchTag);

      // Якщо поле пошуку порожнє — показуємо всі зображення
      const dataToShow = !normalizedSearch
        ? list
        : list.filter((item) => {
          const tagsArray = (item.Tags ?? '')
            .split(',')
            .map((t) => t.trim())
            .filter((t) => t.length > 0);

          return tagsArray.some((t) => {
            const normTag = normalizeTag(t);
            return normTag.startsWith(normalizedSearch);
          });
        });

      return (
        <View style={[styles.listWrapper, isLandscape && styles.listWrapperLandscape]}>
          <Text style={styles.header}>
            All Images
          </Text>

          {/* Пошук по тегу під заголовком */}
          <TextInput
            style={styles.searchInput}
            placeholder="Search images by tag"
            placeholderTextColor="#64748b"
            value={searchTag}
            onChangeText={(text) => { setSearchTag(text) }}
          />

          <FlatList
            key={isLandscape ? 'landscape' : 'portrait'}
            data={dataToShow}
            renderItem={({ item }) => (<Card {...item} />)}
            keyExtractor={(item) => String(item.Id)}
            numColumns={2}
            contentContainerStyle={styles.listContent}
          />
        </View>
      );
    }
    return (
      <View style={styles.center}>
        <Text style={styles.emptyText}>No pictures yet</Text>
      </View>
    );
  };

  return (
    <View style={styles.container}>
      {renderContent()}
    </View>
  );
}

export default Index;

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#050016',
    alignItems: 'stretch',
    alignContent: 'flex-start',
    paddingTop: 20,
    paddingBottom: 70  // Запас під таббар
  },
  header: {
    color: '#c4b5fd',
    fontSize: 22,
    fontWeight: '700',
    marginBottom: 6,
    textAlign: 'center',
    textShadowColor: '#a855f7',
    textShadowOffset: { width: 0, height: 0 },
    textShadowRadius: 10
  },
  listWrapper: {
    flex: 1,
    paddingHorizontal: 4
  },
  listWrapperLandscape: {
    paddingHorizontal: 36
  },
  listContent: {
    paddingBottom: 20
  },
  center: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingTop: 20
  },
  loadingText: {
    color: '#e2e8f0',
    marginTop: 6
  },
  errorText: {
    color: '#fb7185',
    fontWeight: '600'
  },
  emptyText: {
    color: '#64748b'
  },
  searchInput: {
    marginHorizontal: 10,
    marginBottom: 8,
    paddingVertical: 6,
    paddingHorizontal: 10,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: '#22d3ee',
    backgroundColor: '#020617',
    color: '#e2e8f0',
    textAlign: 'left',
    shadowColor: '#4f46e5',
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0.6,
    shadowRadius: 10,
    elevation: 6
  }
});