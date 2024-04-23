function continueReading(articleId) {
    // Находим элементы с конкретным ID статьи
    var trimmedContent = $('#articleContent' + articleId);
    var continueButton = $('#continueReading' + articleId);

    // Проверяем, скрыто ли текущее содержание статьи
    if (trimmedContent.hasClass('article-trimmed-content')) {
        // Если скрыто, то показываем полное содержание и меняем классы
        trimmedContent.removeClass('article-trimmed-content').addClass('article-content');
        continueButton.text('Скрыть'); // Меняем текст кнопки на "Скрыть"
    } else {
        // Если отображается полное содержание, то скрываем его и меняем классы
        trimmedContent.removeClass('article-content').addClass('article-trimmed-content');
        continueButton.text('Продолжить чтение'); // Меняем текст кнопки на "Продолжить чтение"
    }
}