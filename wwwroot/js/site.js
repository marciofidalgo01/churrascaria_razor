
(() => {
    // MENU
    const menuButton = document.querySelector('.menu-toggle');
    const nav = document.querySelector('.main-nav');

    function closeMenu() {
        nav?.classList.remove('is-open');
        menuButton?.setAttribute('aria-expanded', 'false');
    }

    menuButton?.addEventListener('click', () => {
        const opened = nav.classList.toggle('is-open');
        menuButton.setAttribute('aria-expanded', String(opened));
    });

    nav?.querySelectorAll('a').forEach(link =>
        link.addEventListener('click', closeMenu)
    );

    document.addEventListener('keydown', event => {
        if (event.key === 'Escape') closeMenu();
    });

    document.addEventListener('click', event => {
        if (nav && !nav.contains(event.target) &&
            !menuButton?.contains(event.target)) {
            closeMenu();
        }
    });

    // CONFIRMAÇÕES
    document.querySelectorAll('[data-confirm]').forEach(button => {
        button.addEventListener('click', event => {
            if (!window.confirm(button.dataset.confirm)) {
                event.preventDefault();
            }
        });
    });

    // GALERIA
    const dialog = document.querySelector('#galeria-dialog');

    let previousFocus;
    let imageRequestId = 0;

    const imageCache = new Map();

    function preloadImage(url) {
        if (imageCache.has(url)) {
            return imageCache.get(url);
        }

        const promise = new Promise((resolve, reject) => {
            const image = new Image();

            image.decoding = 'async';
            image.src = url;

            image.decode()
                .then(() => resolve(image))
                .catch(() => {
                    if (image.complete && image.naturalWidth > 0) {
                        resolve(image);
                    } else {
                        reject(new Error('Falha ao carregar imagem.'));
                    }
                });
        });

        imageCache.set(url, promise);

        promise.catch(() => imageCache.delete(url));

        return promise;
    }

    document.querySelectorAll('[data-gallery-image]').forEach(link => {

        // Pré-carrega ao passar o mouse ou receber foco.
        link.addEventListener('pointerenter', () => {
            preloadImage(link.href).catch(() => {});
        }, { once: true });

        link.addEventListener('focus', () => {
            preloadImage(link.href).catch(() => {});
        }, { once: true });

        link.addEventListener('click', async event => {
            if (!dialog?.showModal) return;

            event.preventDefault();

            previousFocus = link;

            const requestId = ++imageRequestId;
            const image = dialog.querySelector('img');
            const caption = dialog.querySelector('p');

            if (!dialog.open) {
                dialog.showModal();
            }

            try {
                const loadedImage = await preloadImage(link.href);

                if (requestId !== imageRequestId || !dialog.open) {
                    return;
                }

                image.src = loadedImage.src;
                image.alt = link.dataset.caption || '';

                caption.textContent = link.dataset.caption || '';

            } catch (error) {
                console.error('Erro na galeria:', error);
            }
        });
    });

    dialog?.querySelector('[data-close-dialog]')
        ?.addEventListener('click', () => dialog.close());

    dialog?.addEventListener('click', event => {
        if (event.target === dialog) dialog.close();
    });

    dialog?.addEventListener('close', () => {
        imageRequestId++;
        previousFocus?.focus();
    });

    // FORMULÁRIO
    const form = document.querySelector('[data-booking-form]');

    form?.addEventListener('submit', () => {
        const button = form.querySelector('[data-submit]');

        if (!button) return;

        button.disabled = true;
        button.textContent = 'Enviando solicitação…';
        button.setAttribute('aria-busy', 'true');
    });

    window.addEventListener('pageshow', () => {
        const button = form?.querySelector('[data-submit]');

        if (button) {
            button.disabled = false;
            button.innerHTML =
                'Enviar solicitação <span aria-hidden="true">↗</span>';
            button.removeAttribute('aria-busy');
        }
    });

    // CARROSSEL DO PORTFÓLIO
    document.querySelectorAll('[data-carousel]').forEach(carousel => {
        const slides = carousel.querySelectorAll('.portfolio-slide');
        const counter = carousel.querySelector('[data-carousel-counter]');
        const previous = carousel.querySelector('[data-carousel-prev]');
        const next = carousel.querySelector('[data-carousel-next]');

        if (!slides.length || !counter || !previous || !next) return;

        let currentIndex = 0;

        function showSlide(index) {
            currentIndex = (index + slides.length) % slides.length;

            slides.forEach((slide, i) => {
                slide.classList.toggle('active', i === currentIndex);
            });

            counter.textContent =
                `${currentIndex + 1} / ${slides.length}`;
        }

        previous.addEventListener('click', () => {
            showSlide(currentIndex - 1);
        });

        next.addEventListener('click', () => {
            showSlide(currentIndex + 1);
        });
    });

    // MÁSCARA DE TELEFONE
    document.querySelectorAll('[data-phone-mask]').forEach(campo => {
        campo.addEventListener('input', () => {
            let numeros = campo.value.replace(/\D/g, '').substring(0, 11);

            if (!numeros.length) {
                campo.value = '';
                return;
            }

            if (numeros.length <= 2) {
                campo.value = `(${numeros}`;
                return;
            }

            const ddd = numeros.substring(0, 2);
            const restante = numeros.substring(2);

            if (restante.length > 8) {
                campo.value =
                    `(${ddd}) ${restante.substring(0, 5)}-${restante.substring(5)}`;
            } else {
                campo.value =
                    `(${ddd}) ${restante.substring(0, 4)}${restante.length > 4 ? '-' + restante.substring(4) : ''}`;
            }
        });
    });

})();
