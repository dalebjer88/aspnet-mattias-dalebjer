document.addEventListener("DOMContentLoaded", () => {
    const accordions = document.querySelectorAll("[data-accordion]");

    accordions.forEach((accordion) => {
        const items = accordion.querySelectorAll("[data-accordion-item]");

        items.forEach((item) => {
            item.addEventListener("toggle", () => {
                if (!item.open) {
                    return;
                }

                items.forEach((otherItem) => {
                    if (otherItem !== item) {
                        otherItem.open = false;
                    }
                });
            });
        });
    });

    const faqImageContainer = document.getElementById("faq-image-container");

    if (faqImageContainer) {
        const desktopBreakpoint = window.matchMedia("(min-width: 1024px)");

        const renderFaqImage = () => {
            if (desktopBreakpoint.matches) {
                if (!faqImageContainer.querySelector("img")) {
                    faqImageContainer.innerHTML = `
                        <img
                            src="/images/faqs-image1.svg"
                            alt="Gym training"
                            class="block max-w-full h-auto"
                        />
                    `;
                }
            } else {
                faqImageContainer.innerHTML = "";
            }
        };

        renderFaqImage();
        desktopBreakpoint.addEventListener("change", renderFaqImage);
    }
});