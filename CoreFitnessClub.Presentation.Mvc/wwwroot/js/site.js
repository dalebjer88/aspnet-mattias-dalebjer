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

    const mobileMenuButton = document.getElementById("mobile-menu-button");
    const mobileMenu = document.getElementById("mobile-menu");

    if (mobileMenuButton && mobileMenu) {
        mobileMenuButton.addEventListener("click", () => {
            const isExpanded = mobileMenuButton.getAttribute("aria-expanded") === "true";

            mobileMenu.classList.toggle("hidden");
            mobileMenuButton.classList.toggle("is-open");
            mobileMenuButton.setAttribute("aria-expanded", String(!isExpanded));
        });
    }

    const mobileTrainingButton = document.getElementById("mobile-training-button");
    const mobileTrainingMenu = document.getElementById("mobile-training-menu");
    const mobileTrainingIcon = document.getElementById("mobile-training-icon");

    if (mobileTrainingButton && mobileTrainingMenu) {
        mobileTrainingButton.addEventListener("click", () => {
            const isExpanded = mobileTrainingButton.getAttribute("aria-expanded") === "true";

            mobileTrainingMenu.classList.toggle("hidden");
            mobileTrainingButton.setAttribute("aria-expanded", String(!isExpanded));

            if (mobileTrainingIcon) {
                mobileTrainingIcon.classList.toggle("rotate-180");
            }
        });
    }

    const desktopTrainingButton = document.getElementById("desktop-training-button");
    const desktopTrainingMenu = document.getElementById("desktop-training-menu");

    if (desktopTrainingButton && desktopTrainingMenu) {
        desktopTrainingButton.addEventListener("click", () => {
            const isExpanded = desktopTrainingButton.getAttribute("aria-expanded") === "true";

            desktopTrainingMenu.classList.toggle("hidden");
            desktopTrainingButton.setAttribute("aria-expanded", String(!isExpanded));
        });
    }

    const desktopTrainingButtonXl = document.getElementById("desktop-training-button-xl");
    const desktopTrainingMenuXl = document.getElementById("desktop-training-menu-xl");

    if (desktopTrainingButtonXl && desktopTrainingMenuXl) {
        desktopTrainingButtonXl.addEventListener("click", () => {
            const isExpanded = desktopTrainingButtonXl.getAttribute("aria-expanded") === "true";

            desktopTrainingMenuXl.classList.toggle("hidden");
            desktopTrainingButtonXl.setAttribute("aria-expanded", String(!isExpanded));
        });
    }
});