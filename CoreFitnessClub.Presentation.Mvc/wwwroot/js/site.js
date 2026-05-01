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

    const rememberMeCheckbox = document.querySelector("[data-remember-me-checkbox]");
    const externalRememberMeInput = document.querySelector("#externalRememberMe");

    if (rememberMeCheckbox && externalRememberMeInput) {
        const updateExternalRememberMe = () => {
            externalRememberMeInput.value = rememberMeCheckbox.checked ? "true" : "false";
        };

        updateExternalRememberMe();
        rememberMeCheckbox.addEventListener("change", updateExternalRememberMe);
    }

    const forms = document.querySelectorAll("[data-form-validation]");

    const validateInput = (input) => {
        const isCheckbox = input.type === "checkbox";
        const value = isCheckbox ? (input.checked ? "true" : "false") : input.value.trim();
        const fieldName = input.getAttribute("name");
        const form = input.closest("form");
        const validationMessage = form?.querySelector(
            `[data-valmsg-for='${fieldName}']`,
        );

        if (!validationMessage) {
            return true;
        }

        let message = "";

        if (!message && input.dataset.valRequired) {
            if (isCheckbox && !input.checked) {
                message = input.dataset.valRequired;
            }

            if (!isCheckbox && value.length === 0) {
                message = input.dataset.valRequired;
            }
        }

        if (!message && input.dataset.valRange && isCheckbox && !input.checked) {
            message = input.dataset.valRange;
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

        if (
            !message &&
            input.dataset.valLengthMax &&
            value.length > Number(input.dataset.valLengthMax)
        ) {
            message = input.dataset.valLength;
        }

        if (!message && input.dataset.valEqualto && value.length > 0) {
            const otherFieldName = input.dataset.valEqualtoOther?.replace("*.", "");
            const otherInput = form?.querySelector(`[name='${otherFieldName}']`);

            if (otherInput && value !== otherInput.value) {
                message = input.dataset.valEqualto;
            }
        }

        validationMessage.textContent = message;
        validationMessage.style.color = "red";

        return message.length === 0;
    };

    forms.forEach((form) => {
        const inputs = form.querySelectorAll("input[data-val='true'], textarea[data-val='true']");

        inputs.forEach((input) => {
            input.addEventListener("input", () => {
                validateInput(input);

                const relatedInputs = form.querySelectorAll(
                    `input[data-val-equalto-other='*.${input.name}']`,
                );

                relatedInputs.forEach((relatedInput) => {
                    validateInput(relatedInput);
                });
            });

            input.addEventListener("blur", () => {
                validateInput(input);
            });
        });
    });

    const adminClassForms = document.querySelectorAll("[data-admin-class-validation]");

    const getTodayValue = () => {
        const today = new Date();
        const year = today.getFullYear();
        const month = String(today.getMonth() + 1).padStart(2, "0");
        const day = String(today.getDate()).padStart(2, "0");

        return `${year}-${month}-${day}`;
    };

    const getCurrentTimeValue = () => {
        const now = new Date();
        const hours = String(now.getHours()).padStart(2, "0");
        const minutes = String(now.getMinutes()).padStart(2, "0");

        return `${hours}:${minutes}`;
    };

    adminClassForms.forEach((form) => {
        const dateInput = form.querySelector("[data-class-date]");
        const startTimeInput = form.querySelector("[data-class-start-time]");
        const endTimeInput = form.querySelector("[data-class-end-time]");

        if (!dateInput || !startTimeInput || !endTimeInput) {
            return;
        }

        const setValidationMessage = (input, message) => {
            const fieldName = input.getAttribute("name");
            const validationMessage = form.querySelector(
                `[data-valmsg-for='${fieldName}']`,
            );

            if (!validationMessage) {
                return;
            }

            validationMessage.textContent = message;
            validationMessage.style.color = "red";
        };

        const validateAdminClassTimes = () => {
            const dateIsValid = validateInput(dateInput);
            const startTimeIsValid = validateInput(startTimeInput);
            const endTimeIsValid = validateInput(endTimeInput);

            if (!dateIsValid || !startTimeIsValid || !endTimeIsValid) {
                return false;
            }

            let isValid = true;

            const todayValue = getTodayValue();
            const currentTimeValue = getCurrentTimeValue();

            if (dateInput.value && dateInput.value < todayValue) {
                setValidationMessage(dateInput, "Class date cannot be in the past.");
                isValid = false;
            }

            if (
                dateInput.value === todayValue &&
                startTimeInput.value &&
                startTimeInput.value <= currentTimeValue
            ) {
                setValidationMessage(
                    startTimeInput,
                    "Start time must be later than the current time.",
                );
                isValid = false;
            }

            if (
                startTimeInput.value &&
                endTimeInput.value &&
                endTimeInput.value <= startTimeInput.value
            ) {
                setValidationMessage(
                    endTimeInput,
                    "End time must be later than start time.",
                );
                isValid = false;
            }

            return isValid;
        };

        [dateInput, startTimeInput, endTimeInput].forEach((input) => {
            input.addEventListener("input", validateAdminClassTimes);
            input.addEventListener("change", validateAdminClassTimes);
            input.addEventListener("blur", validateAdminClassTimes);
        });

        form.addEventListener("submit", (event) => {
            if (!validateAdminClassTimes()) {
                event.preventDefault();
            }
        });
    });

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
