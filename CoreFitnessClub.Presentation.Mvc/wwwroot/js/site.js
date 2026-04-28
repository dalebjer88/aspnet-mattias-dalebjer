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
      const isExpanded =
        mobileMenuButton.getAttribute("aria-expanded") === "true";

      mobileMenu.classList.toggle("hidden");
      mobileMenuButton.classList.toggle("is-open");
      mobileMenuButton.setAttribute("aria-expanded", String(!isExpanded));
    });
  }

  const mobileTrainingButton = document.getElementById(
    "mobile-training-button",
  );
  const mobileTrainingMenu = document.getElementById("mobile-training-menu");
  const mobileTrainingIcon = document.getElementById("mobile-training-icon");

  if (mobileTrainingButton && mobileTrainingMenu) {
    mobileTrainingButton.addEventListener("click", () => {
      const isExpanded =
        mobileTrainingButton.getAttribute("aria-expanded") === "true";

      mobileTrainingMenu.classList.toggle("hidden");
      mobileTrainingButton.setAttribute("aria-expanded", String(!isExpanded));

      if (mobileTrainingIcon) {
        mobileTrainingIcon.classList.toggle("rotate-180");
      }
    });
  }

  const desktopTrainingButton = document.getElementById(
    "desktop-training-button",
  );
  const desktopTrainingMenu = document.getElementById("desktop-training-menu");

  if (desktopTrainingButton && desktopTrainingMenu) {
    desktopTrainingButton.addEventListener("click", () => {
      const isExpanded =
        desktopTrainingButton.getAttribute("aria-expanded") === "true";

      desktopTrainingMenu.classList.toggle("hidden");
      desktopTrainingButton.setAttribute("aria-expanded", String(!isExpanded));
    });
  }

  const desktopTrainingButtonXl = document.getElementById(
    "desktop-training-button-xl",
  );
  const desktopTrainingMenuXl = document.getElementById(
    "desktop-training-menu-xl",
  );

  if (desktopTrainingButtonXl && desktopTrainingMenuXl) {
    desktopTrainingButtonXl.addEventListener("click", () => {
      const isExpanded =
        desktopTrainingButtonXl.getAttribute("aria-expanded") === "true";

      desktopTrainingMenuXl.classList.toggle("hidden");
      desktopTrainingButtonXl.setAttribute(
        "aria-expanded",
        String(!isExpanded),
      );
    });
    }

    const forms = document.querySelectorAll("[data-form-validation]");

    forms.forEach((form) => {
        const inputs = form.querySelectorAll("input[data-val='true']");

        inputs.forEach((input) => {
            input.addEventListener("input", () => {
                validateInput(input);
            });

            input.addEventListener("blur", () => {
                validateInput(input);
            });
        });
    });

    const validateInput = (input) => {
        const value = input.value.trim();
        const fieldName = input.getAttribute("name");
        const validationMessage = document.querySelector(
            `[data-valmsg-for='${fieldName}']`,
        );

        if (!validationMessage) {
            return true;
        }

        let message = "";

        if (!message && input.dataset.valRequired && value.length === 0) {
            message = input.dataset.valRequired;
        }

        if (!message && input.dataset.valEmail && value.length > 0) {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            if (!emailPattern.test(value)) {
                message = input.dataset.valEmail;
            }
        }

        if (!message && input.dataset.valPhone && value.length > 0) {
            const phonePattern = /^[0-9+\-\s()]+$/;

            if (!phonePattern.test(value)) {
                message = input.dataset.valPhone;
            }
        }

        if (!message && input.dataset.valRegex && value.length > 0) {
            const pattern = new RegExp(input.dataset.valRegexPattern);

            if (!pattern.test(value)) {
                message = input.dataset.valRegex;
            }
        }

        if (!message && input.dataset.valLengthMax && value.length > Number(input.dataset.valLengthMax)) {
            message = input.dataset.valLength;
        }

        validationMessage.textContent = message;
        validationMessage.style.color = "red";

        return message.length === 0;
    };

    const profileImageInput = document.querySelector("#profileImage");

    if (profileImageInput) {
        profileImageInput.addEventListener("change", (event) => {
            const file = event.target.files[0];
            const fileNameLabel = document.querySelector("#profileImageFileName");
            const imagePreview = document.querySelector("#accountProfileImage");

            if (fileNameLabel) {
                fileNameLabel.textContent = file ? file.name : "Upload Profile Image";
            }

            if (!file || !imagePreview) {
                return;
            }

            const reader = new FileReader();

            reader.onload = (event) => {
                imagePreview.src = event.target.result;
            };

            reader.readAsDataURL(file);
        });
    }
});
